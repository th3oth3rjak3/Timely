namespace Timely.Domain.Features.GlobalTimer;

/// <summary>
/// TimerStatus represents the current state of the global timer.
/// </summary>
public enum TimerStatus
{
    /// <summary>
    /// Represents a stopped timer.
    /// </summary>
    Stopped,

    /// <summary>
    /// Represents a running timer.
    /// </summary>
    Running,

    /// <summary>
    /// Represents a paused timer.
    /// </summary>
    Paused,

    /// <summary>
    /// Represents a timer that has finished.
    /// </summary>
    Finished
}

/// <summary>
/// Extensions to simplify some TimerStatus operations.
/// </summary>
public static class TimerStatusExtensions
{
    /// <summary>
    /// True when the timer is paused, otherwise false.
    /// </summary>
    public static bool IsPaused(this TimerStatus status) => status == TimerStatus.Paused;

    /// <summary>
    /// True when the timer is running, otherwise false.
    /// </summary>
    public static bool IsRunning(this TimerStatus status) => status == TimerStatus.Running;
}