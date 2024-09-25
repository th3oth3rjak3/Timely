namespace Timely;

using Foundation;

/// <summary>
/// An application delegate.
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    /// <summary>
    /// Create a new Maui App.
    /// </summary>
    /// <returns></returns>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
