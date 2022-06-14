using Foundation;

namespace DewIt.Client;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => EntryPoint.CreateMauiApp();
}
