namespace Timely.Components.Pages;
using Microsoft.AspNetCore.Components;

using MudBlazor;

using Timely.Domain.Extensions;
using Timely.Domain.Features.GlobalTimer;

/// <summary>
/// The page that shows the global timer.
/// </summary>
public partial class TimerPage : IDisposable
{
    /// <summary>
    /// The timer service that manages the global timer.
    /// </summary>
    [Inject]
    public required IGlobalTimerService TimerService { get; set; }

    /// <summary>
    /// The snackbar used to show alerts to the user.
    /// </summary>
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    /// <summary>
    /// The time picker component.
    /// </summary>
    private MudTimePicker timePicker = default!;

    /// <summary>
    /// A backing field to hold timer state on timer update events.
    /// </summary>
    private GlobalTimerState _stateFromService = new();

    /// <summary>
    /// Timer State from the timer service that is updated when
    /// events are fired from the timer tick.
    /// </summary>
    private GlobalTimerState StateFromService
    {
        get => _stateFromService;
        set
        {
            _stateFromService = value;
            TimerDuration = value.NextDuration ?? value.Duration;
        }
    }

    /// <summary>
    /// Where timer duration changes are temporarily stored when a 
    /// user changes the duration with the Time Picker component.
    /// </summary>
    private TimeSpan? TimerDuration { get; set; }

    /// <summary>
    /// Register event handlers for timer state changes and set the initial
    /// timer state on initialization.
    /// </summary>
    protected override void OnInitialized()
    {
        TimerService.OnStateChanged += StateChanged;
        StateFromService = TimerService.GetInitialState();
    }

    /// <summary>
    /// Called when the user presses the "Save" button. This sets the timer duration with the
    /// global timer service. This is called via a button instead of the TimerDuration setter
    /// because there are a lot of OnChange events that happen when a user is selecting
    /// a time and it was lagging a lot.
    /// </summary>
    private async void SetTimerDuration()
    {
        await timePicker.CloseAsync();

        if (!TimerDuration.HasValue)
        {
            return;
        }

        TimerService.SetTimerDuration(TimerDuration);
        var alertMessage = $"Updated duration to {TimerDuration!.Value.ToDisplay()}";
        Snackbar.Add(alertMessage, Severity.Success);
    }

    /// <summary>
    /// Unregister the StateChanged event handlers and dispose.
    /// </summary>
    public void Dispose()
    {
        TimerService.OnStateChanged -= StateChanged;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Handle timer state changes from the global timer event.
    /// </summary>
    /// <param name="state">The most recent timer state</param>
    private void StateChanged(GlobalTimerState state)
    {
        _stateFromService = state;
        _ = InvokeAsync(StateHasChanged);
    }

    private string TimeRemainingDisplay => _stateFromService.Remaining.ToDisplay();
    private string TimerDurationDisplay => TimerDuration.DynamicTimeDisplayFormat();

    /// <summary>
    /// Start the timer.
    /// </summary>
    public void StartTimer()
    {
        if (_stateFromService.Duration == TimeSpan.Zero)
        {
            Snackbar.Add("Please set a time for the timer.", Severity.Error);
            return;
        }

        TimerService.StartTimer();
    }

    /// <summary>
    /// Pause the timer.
    /// </summary>
    public void PauseTimer() => TimerService.PauseTimer();

    /// <summary>
    /// Reset the timer.
    /// </summary>
    public void ResetTimer() => TimerService.ResetTimer();

    /// <summary>
    /// Resume the timer when paused.
    /// </summary>
    public void ResumeTimer() => TimerService.ResumeTimer();
}