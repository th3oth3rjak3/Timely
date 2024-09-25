namespace Timely.Domain.Features.GlobalTimer;

using System.Collections.Concurrent;
using System.Timers;

/// <inheritdoc cref="IGlobalTimerService"/>
public class GlobalTimerService : IGlobalTimerService
{
    /// <summary>
    /// Create a new Global Timer Service.
    /// </summary>
    public GlobalTimerService()
    {
        Timer.Elapsed += PublishTimerUpdate;
        State = new() { Duration = TimeSpan.FromHours(1) };
    }

    /// <summary>
    /// The timer instance that emits events at a routine interval. AutoReset is set
    /// to true so that it will continue emitting events at the specified interval.
    /// </summary>
    private Timer Timer { get; set; } = new() { AutoReset = true };

    /// <summary>
    /// Internal state used keep track of the timer information.
    /// </summary>
    private GlobalTimerState State { get; set; }

    /// <summary>
    /// A queue that holds requested timer duration updates until the timer is stopped or reset.
    /// </summary>
    private readonly ConcurrentStack<TimeSpan?> DurationUpdatesRequested = new();

    ///<inheritdoc />
    public event Action<GlobalTimerState>? OnTimerFinished;

    ///<inheritdoc />
    public event Action<GlobalTimerState>? OnStateChanged;

    ///<inheritdoc />
    public void StartTimer()
    {
        State.Status = TimerStatus.Running;
        State.StartTime = DateTime.Now;
        State.LastUpdateTime = State.StartTime;
        Timer.Start();
        OnStateChanged?.Invoke(State);
    }

    ///<inheritdoc />
    public void PauseTimer()
    {
        Timer.Stop();
        State.Status = TimerStatus.Paused;
        State.LastUpdateTime = DateTime.Now;
        OnStateChanged?.Invoke(State);
    }

    ///<inheritdoc />
    public void ResumeTimer()
    {
        Timer.Start();
        State.Status = TimerStatus.Running;
        State.StartTime = DateTime.Now.AddSeconds(-State.Elapsed.TotalSeconds);
        State.LastUpdateTime = DateTime.Now;
        OnStateChanged?.Invoke(State);
    }

    ///<inheritdoc />
    public void ResetTimer()
    {
        State.Status = TimerStatus.Stopped;
        State.LastUpdateTime = default;
        State.StartTime = default;
        if (State.NextDuration.HasValue)
        {
            State.Duration = State.NextDuration.Value;
            State.NextDuration = null;
        }

        OnStateChanged?.Invoke(State);
    }

    ///<inheritdoc />
    public void SetTimerDuration(TimeSpan? duration)
    {
        var newDuration = duration ?? TimeSpan.Zero;

        if (!State.IsStopped)
        {
            DurationUpdatesRequested.Push(newDuration);
            return;
        }

        State.Duration = newDuration;
        State.NextDuration = null;
        OnStateChanged?.Invoke(State);
    }

    /// <summary>
    /// After each interval, PublishTimerUpdate observes the duration that has elapsed
    /// and emits the updates based on the current elapsed time.
    /// </summary>
    private void PublishTimerUpdate(object? sender, ElapsedEventArgs e)
    {
        var elapsed = e.SignalTime - State.StartTime;
        State.LastUpdateTime = DateTime.Now;

        // Manage pending duration change requests
        if (DurationUpdatesRequested.TryPop(out var updatedDuration))
        {
            State.NextDuration = updatedDuration;
            DurationUpdatesRequested.Clear();
        }

        // Timer Finished condition
        if (elapsed >= State.Duration)
        {
            State.Status = TimerStatus.Finished;
            Timer.Stop();
            OnTimerFinished?.Invoke(State);
            ResetTimer();
        }
        // Normal update condition
        else
        {

            State.Status = TimerStatus.Running;
            OnStateChanged?.Invoke(State);
        }
    }

    ///<inheritdoc />
    public GlobalTimerState GetInitialState() => State;
}
