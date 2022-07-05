using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Application = Microsoft.Maui.Controls.Application;
using Toast = CommunityToolkit.Maui.Alerts.Toast;
using CommunityToolkit.Maui.Core.Extensions;

// TODO: Verify that System.WINDOWS.Input works on MAC
// if not, then move ICommand and all it's required kibble over to Model
using System.Windows.Input;
using DewIt.Client.Features.DataTemplates;
using DewIt.Model.DataTypes;
using DewIt.Model.Events;
using DewIt.Model.Persistence;
using Font = Microsoft.Maui.Font;

namespace DewIt.Client.Features.MainWindow;

public class MainPageViewModel : ObservableObject
{
    private ObservableCollection<LaneInfo> _lanes;
    private Card _dragCard;
    private int _position;
    public readonly ICardRepository CardsRepository;
    public readonly ILaneRepository LanesRepository;

    public static readonly BindableProperty EventAggregatorProperty = BindableProperty.Create(nameof(EventAggregator),
        typeof(IEventAggregator), typeof(CardView));

    private IEventAggregator _eventAggregator;

    public IEventAggregator EventAggregator
    {
        get => _eventAggregator;
        set
        {
            if (value == _eventAggregator) return;
            _eventAggregator = value;
            OnPropertyChanged(nameof(EventAggregator));
        }
    }

    public MainPageViewModel(ICardRepository cardsRepository, ILaneRepository lanesRepository, IEventAggregator events)
    {
        this.EventAggregator = events;
        this.CardsRepository = cardsRepository;
        this.LanesRepository = lanesRepository;
        RefreshCommand.Execute(null);

        EventAggregator.GetEvent<DeleteCardEvent>().Subscribe(async card => await DeleteCard(card));
    }

    public ICommand RefreshCommand => new AsyncRelayCommand(RefreshCollection);

    public ICommand DropCommand => new AsyncRelayCommand<LaneInfo>(async laneInfo =>
    {
        if (_dragCard is null || laneInfo.Lane.Cards.Count >= laneInfo.Lane.MaxWorkInProgress) return;

        var cardToUpdate = await CardsRepository.GetItem(_dragCard.UUID);
        if (cardToUpdate is not null)
        {
            cardToUpdate.LaneUUID = laneInfo.Lane.UUID;
            await CardsRepository.UpdateItem(cardToUpdate);
        }

        await RefreshCollection();
        Position = laneInfo.Index;
    });

    public ICommand DragStartingCommand => new RelayCommand<Card>(card => { _dragCard = card; });

    public ICommand DropCompletedCommand => new RelayCommand(() => { _dragCard = null; });

    public ICommand AddColumn => new AsyncRelayCommand(async () =>
    {
        var columnName = await UserPromptAsync("New column", "Enter column name", Keyboard.Default);
        if (string.IsNullOrWhiteSpace(columnName)) return;

        int wip;
        do
        {
            var wipString = await UserPromptAsync("New column", "Enter column WIP", Keyboard.Numeric);
            if (string.IsNullOrWhiteSpace(wipString)) return;

            int.TryParse(wipString, out wip);
        } while (wip < 0);

        var column = new Lane { DisplayName = columnName, MaxWorkInProgress = wip, Order = _lanes.Count + 1 };
        await LanesRepository.SaveItem(column);
        await RefreshCollection();
        await ToastAsync("Column is added");
    });

    public ICommand AddCard => new AsyncRelayCommand<Guid>(async laneUUID =>
    {
        var column = await LanesRepository.GetItem(laneUUID);
        var columnInfo = new LaneInfo(0, column);
        if (columnInfo.IsMaxWIPReached)
        {
            await WipReachedToastAsync("WIP is reached");
            return;
        }

        var cardName = await UserPromptAsync("New card", "Enter card name", Keyboard.Default);
        if (string.IsNullOrWhiteSpace(cardName)) return;

        var cardDescription = await UserPromptAsync("New card", "Enter card description", Keyboard.Default);
        await CardsRepository.SaveItem(new Card
        {
            DisplayName = cardName,
            Description = cardDescription,
            LaneUUID = laneUUID,
            Order = column.Cards.Count + 1
        });

        await RefreshCollection();
        await ToastAsync("Card is added");
    });

    public ICommand DeleteCardCommand => new AsyncRelayCommand<Card>(async card => { await DeleteCard(card); });

    private async Task DeleteCard(Card card)
    {
        var result = await AlertAsync("Delete card", $"Do you want to delete card \"{card.DisplayName}\"?");
        if (!result) return;

        bool isCancelled = false;
        Action action = () => isCancelled = true;
        await SnackbarAsync("The card is about to be removed", "Cancel", action);
        if (isCancelled)
        {
            await ToastAsync("Task is cancelled");
        }
        else
        {
            await CardsRepository.DeleteItem(card.UUID);
            await RefreshCollection();
        }
    }

    public ICommand DeleteColumnCommand => new Command<LaneInfo>(async laneInfo =>
    {
        var result = await AlertAsync("Delete column",
            $"Do you want to delete column \"{laneInfo.Lane.DisplayName}\" and all its cards?");
        if (!result) return;

        Lanes.Remove(laneInfo);
        var isCancelled = false;

        void cancelAction()
        {
            Lanes.Add(laneInfo);
            isCancelled = true;
        }

        // alert the user that it is about to be removed... doesn't work on Windows :)
        await SnackbarAsync("The column is about to be removed...", "Cancel", cancelAction);

        if (!isCancelled)
        {
            await LanesRepository.CascadeDelete(laneInfo.Lane);
        }

        await RefreshCollection();
    });

    // holds the entire datamodel
    public ObservableCollection<LaneInfo> Lanes
    {
        get => _lanes;
        set => SetProperty(ref _lanes, value);
    }

    public int Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }

    private async Task RefreshCollection()
    {
        var items = await LanesRepository.GetAll();
        Lanes = items
            .OrderBy(c => c.Order)
            .ToList()
            .Select(OrderCards)
            .ToObservableCollection();
        Position = 0;
    }

    private static LaneInfo OrderCards(Lane c, int columnNumber)
    {
        c.Cards = c.Cards.OrderBy(card => card.Order).ToList();
        return new LaneInfo(columnNumber, c);
    }

    private static Task<bool> AlertAsync(string title, string message)
    {
        return Application.Current!.MainPage!.DisplayAlert(title, message, "Yes", "No");
    }

    private static Task<string> UserPromptAsync(string title, string message, Keyboard keyboard)
    {
        return Application.Current!.MainPage!.DisplayPromptAsync(title, message, keyboard: keyboard);
    }

    private static Task SnackbarAsync(string title, string buttonText, Action buttonAction)
    {
        var options = new SnackbarOptions
        {
            BackgroundColor = Colors.Red,
            TextColor = Colors.Green,
            ActionButtonTextColor = Colors.Yellow,
            CornerRadius = new CornerRadius(10),
            Font = Font.SystemFontOfSize(14),
            ActionButtonFont = Font.SystemFontOfSize(14),
            CharacterSpacing = 0.5
        };
        return Application.Current!.MainPage!.DisplaySnackbar(title, buttonAction, buttonText, TimeSpan.FromSeconds(3),
            options);
    }

    private static Task ToastAsync(string title)
    {
        return Toast.Make(title, ToastDuration.Long).Show();
    }

    private static Task WipReachedToastAsync(string title)
    {
        return Toast.Make(title, ToastDuration.Long, 26d).Show();
    }
}