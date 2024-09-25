namespace Timely.Components.Layout;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using MudBlazor;

using Timely.Domain.Extensions;
using Timely.Domain.Features.GlobalTimer;

/// <summary>
/// MainLayout is the primary layout which wraps around various pages.
/// </summary>
public partial class MainLayout : IDisposable
{
    /// <summary>
    /// The timer service to get global timer information.
    /// </summary>
    [Inject]
    public required IGlobalTimerService TimerService { get; set; }

    /// <summary>
    /// The snack bar used to display alerts.
    /// </summary>
    [Inject]
    public required ISnackbar SnackBar { get; set; }

    /// <summary>
    /// The JS Runtime to interact with audio components.
    /// </summary>
    [Inject]
    public required IJSRuntime JsRuntime { get; set; }

    /// <summary>
    /// State for the sidebar. If true, the sidebar is open.
    /// </summary>
    private bool _drawerOpen = true;

    private bool _timerSoundPlaying = false;

    /// <summary>
    /// Subscribe to TimerFinished updates to display snack bar alerts.
    /// </summary>
    protected override void OnInitialized() => TimerService.OnTimerFinished += TimerFinished;

    /// <summary>
    /// Manually dispose event handlers to prevent memory leaks.
    /// </summary>
    public void Dispose()
    {
        TimerService.OnTimerFinished -= TimerFinished;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// When the timer finishes, show a snack bar message to let the user know.
    /// This snack bar message requires user interaction to dismiss since it's a primary feature.
    /// </summary>
    private async void TimerFinished(GlobalTimerState state)
    {
        // Show the timer message
        var message = $"Beep beep! Your {state.Duration.ToDisplay()} timer has finished!";

        _ = SnackBar.Add(
            message,
            Severity.Info,
            opts =>
            {
                opts.RequireInteraction = true;
                opts.ShowCloseIcon = false;
                opts.Action = "Stop";
                opts.ActionColor = Color.Error;
                opts.Onclick = snackbar =>
                {
                    _timerSoundPlaying = false;
                    return Task.CompletedTask;
                };

            });

        // Make a beeping sound with the audio file. Audio component is in the MainLayout.razor file.
        // https://excetionnotfound.net/how-to-play-a-sound-with-blazor-and-javascript/

        _timerSoundPlaying = true;

        while (_timerSoundPlaying)
        {
            await JsRuntime.InvokeAsync<string>("PlayAudio", "beep");
        }

        // Update state
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Toggle the drawer from opened to closed or the other way around.
    /// </summary>
    private void ToggleDrawer() => _drawerOpen = !_drawerOpen;
}