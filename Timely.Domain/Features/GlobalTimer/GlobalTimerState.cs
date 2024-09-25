namespace Timely.Domain.Features.GlobalTimer;

using Timely.Domain.Extensions;

/// <summary>
/// Global Timer State contains all the important information about a global timer.
/// </summary>
public class GlobalTimerState
{

    /// <summary>
    /// A backing field that retains the original start time.
    /// </summary>
    private DateTime _startTime;

    /// <summary>
    /// A backing field that retains the last update time.
    /// </summary>
    private DateTime _lastUpdateTime;

    /// <summary>
    /// When the timer was started.
    /// </summary>
    public DateTime StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            SetElapsedTime();
        }
    }

    /// <summary>
    /// When the timer was last updated.
    /// </summary>
    public DateTime LastUpdateTime
    {
        get => _lastUpdateTime;
        set
        {
            _lastUpdateTime = value;
            SetElapsedTime();
        }
    }

    /// <summary>
    /// The amount of time that has elapsed.
    /// </summary>
    public TimeSpan Elapsed { get; private set; } = TimeSpan.Zero;

    /// <summary>
    /// The amount of time for the timer to run.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// The next duration to set the timer to after the current
    /// timer has finished.
    /// </summary>
    public TimeSpan? NextDuration { get; set; }

    /// <summary>
    /// The amount of time until the timer is finished.
    /// </summary>
    public TimeSpan Remaining => TimeSpanExtensions.Maximum(Duration - Elapsed, TimeSpan.Zero);

    /// <summary>
    /// The current status of the timer.
    /// </summary>
    public TimerStatus Status { get; set; } = TimerStatus.Stopped;

    /// <summary>
    /// True when the timer is actively running, otherwise false.
    /// </summary>
    public bool IsRunning => Status == TimerStatus.Running;

    /// <summary>
    /// True when the timer is actively paused, otherwise false.
    /// </summary>
    public bool IsPaused => Status == TimerStatus.Paused;

    /// <summary>
    /// True when the timer is actively stopped, otherwise false.
    /// </summary>
    public bool IsStopped => Status == TimerStatus.Stopped;

    /// <summary>
    /// SetElapsedTime calculates the amount of time elapsed and saves it into the backing field.
    /// This is used because when resuming a timer, we only know how long the timer was supposed to go
    /// and how much time has elapsed. So, we can calculate when the start time should be based on the 
    /// elapsed time to only run for the amount of time remaining from the original duration.
    /// Example: for a 10 second timer, if 5 seconds have elapsed, we can set the start time to DateTime.Now.AddSeconds(-5) to have
    /// only 5 seconds remaining on the timer.
    /// </summary>
    private void SetElapsedTime()
    {
        // Guard clause to prevent erroneous conditions when on of these isn't set.
        if (_startTime == default || _lastUpdateTime == default)
        {
            Elapsed = TimeSpan.Zero;
            return;
        }

        Elapsed = LastUpdateTime - StartTime;
    }
}
