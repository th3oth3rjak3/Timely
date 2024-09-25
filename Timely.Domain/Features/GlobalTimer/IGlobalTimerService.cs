
namespace Timely.Domain.Features.GlobalTimer;

/// <summary>
/// The service that manages a single application-wide timer 
/// which persists between page navigation.
/// </summary>
public interface IGlobalTimerService
{
    /// <summary>
    /// An event that is raised when the global timer finishes.
    /// </summary>
    event Action<GlobalTimerState>? OnTimerFinished;

    /// <summary>
    /// An event that is raised when the global timer state is updated
    /// for subscribers to get periodic updates during a timer tick.
    /// </summary>
    event Action<GlobalTimerState>? OnStateChanged;

    /// <summary>
    /// Pause timer execution.
    /// </summary>
    void PauseTimer();

    /// <summary>
    /// Reset the timer to the original duration, or the next duration if one was
    /// set during timer execution.
    /// </summary>
    void ResetTimer();

    /// <summary>
    /// Resume the timer from where it left off when paused.
    /// </summary>
    void ResumeTimer();

    /// <summary>
    /// Set the timer duration. When the timer is actively running, 
    /// duration changes are queued. The last duration is the one that
    /// is used when the timer is reset.
    /// </summary>
    /// <param name="duration">The duration to set the timer to.</param>
    void SetTimerDuration(TimeSpan? duration);

    /// <summary>
    /// Start the timer countdown.
    /// </summary>
    void StartTimer();

    /// <summary>
    /// When initializing UI components, initial state may be necessary to display
    /// values to a user. Since an event may not have been fired yet use this to
    /// get the initial values to show the user.
    /// </summary>
    /// <returns>The current global timer state.</returns>
    GlobalTimerState GetInitialState();
}