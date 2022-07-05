using CommunityToolkit.Mvvm.Input;
using DewIt.Model.DataTypes;
using System.Windows.Input;
using DewIt.Model.Events;

namespace DewIt.Client.Features.DataTemplates;

public partial class CardView
{
    public static readonly BindableProperty EventAggregatorProperty = BindableProperty.Create(nameof(EventAggregator),
        typeof(IEventAggregator), typeof(CardView));

    public IEventAggregator EventAggregator
    {
        get => (IEventAggregator)GetValue(EventAggregatorProperty);
        set => SetValue(EventAggregatorProperty, value);
    }
    

    public CardView()
    {
        InitializeComponent();
    }

    
    private ICommand _deleteButtonCommand;

    public ICommand DeleteButtonCommand
    {
        get => _deleteButtonCommand ??= DeleteCard;
        set
        {
            _deleteButtonCommand = value;
            this.OnPropertyChanged(nameof(DeleteButtonCommand));
        }
    }


    private ICommand DeleteCard => new RelayCommand<Card>(OnCardDeleteClicked);

    private void OnCardDeleteClicked(Card card)
    {
        System.Diagnostics.Debug.WriteLine("Delete Card clicked.");
        EventAggregator?.GetEvent<DeleteCardEvent>().Publish(card);
    }
}