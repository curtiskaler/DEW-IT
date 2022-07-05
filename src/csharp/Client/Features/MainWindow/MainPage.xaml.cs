using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using DewIt.Model.Persistence;

using Font = Microsoft.Maui.Font;

namespace DewIt.Client.Features.MainWindow;

public partial class MainPage
{
    private readonly IResource dataSource;

    public MainPage(MainPageViewModel viewModel, IResource database)
	{
        System.Diagnostics.Debug.WriteLine("MainPage!");
        InitializeComponent();
        BindingContext = viewModel;
        ResetButton ??= new Button();
        this.dataSource = database;
    }

    private async void ResetButton_OnClicked(object sender, EventArgs e)
    {
        var options = new SnackbarOptions
        {
            BackgroundColor = Colors.Red,
            TextColor = Colors.Green,
            ActionButtonTextColor = Colors.Yellow,
            CornerRadius = new CornerRadius(10),
            Font = Font.SystemFontOfSize(14),
        };

        await ResetButton.DisplaySnackbar(
            "All your data will be deleted in 3 seconds. Application will be closed",
            DeleteDbAndCloseApp,
            "Confirm and delete immediately",
            TimeSpan.FromSeconds(5),
            options);
    }

    private void DeleteDbAndCloseApp()
    {
        var dbPath = dataSource.GetPath();
        dataSource.DeleteResource(dbPath);
        Environment.Exit(0);
    }
}

