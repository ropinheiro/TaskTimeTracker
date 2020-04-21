using System;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// The part responsible for the Task's tracking logic.
    /// Should NOT have anything regarding e.g. UI rendering.
    /// </summary>
    public class TaskTracker
    {
        /// <summary>
        /// Date when this task started to be tracked.
        /// </summary>
        public DateTime? DateSinceLastStart { get; set; }

        /// <summary>
        /// Amount of time in seconds spent in previous start-stop cycles
        /// of the timer. It should be added to the current iteration (if
        /// timer is enabled for this task) to get the total spent time.
        /// </summary>
        public long TimePreviouslySpentInSeconds { get; set; }

        /// <summary>
        /// Amount of time in seconds spent in current start-stop cycle
        /// of the timer. Will be zero if timer is stopped.
        /// </summary>
        public long CurrentTimeSpentInSeconds
        {
            get
            {
                if ( !DateSinceLastStart.HasValue )
                    return 0;

                return (long)( DateTime.Now - DateSinceLastStart.Value ).TotalSeconds;
            }
        }

        /// <summary>
        /// Cumulative amount of time spent so far, in seconds.
        /// </summary>
        public long TotalTimeSpentInSeconds
        {
            get
            {
                return TimePreviouslySpentInSeconds + CurrentTimeSpentInSeconds;
            }
        }

        public TaskTracker()
        {
            TimePreviouslySpentInSeconds = 0;
        }

        public TaskTracker( long timeSpentInSeconds )
        {
            TimePreviouslySpentInSeconds = timeSpentInSeconds;
        }

        /// <summary>
        /// React to the timer being started for this task.
        /// </summary>
        public void TimerStarted()
        {
            DateSinceLastStart = DateTime.Now;
        }

        /// <summary>
        /// React to the timer being stopped for this task.
        /// </summary>
        public void TimerStopped()
        {
            TimePreviouslySpentInSeconds += CurrentTimeSpentInSeconds;
            DateSinceLastStart = null;
        }
    }
}
