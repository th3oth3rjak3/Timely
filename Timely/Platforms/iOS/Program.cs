namespace Timely;

using UIKit;

/// <summary>
/// The application entry point class.
/// </summary>
public class Program
{
    /// <summary>
    /// This is the main entry point of the application.
    /// </summary>
    /// <param name="args">Command line args.</param>
    private static void Main(string[] args) =>
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
}
