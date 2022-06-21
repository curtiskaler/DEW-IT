using DewIt.Client.infrastructure;

namespace DewIt.Client;

public static class EntryPoint
{
	public static MauiApp CreateMauiApp()
	{
        System.Diagnostics.Debug.WriteLine("MauiProgram!");
		var builder = MauiApp.CreateBuilder();

		builder
			.UseMauiApp<DewItApp>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		return builder.Build();
	}
}
