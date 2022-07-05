using Foundation;

namespace DewIt.Client;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => EntryPoint.CreateMauiApp();
}
