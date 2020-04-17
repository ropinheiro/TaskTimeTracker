namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// The part responsible for the Task's tracking logic.
    /// Should NOT have anything regarding e.g. UI rendering.
    /// </summary>
    public class TaskTracker
    {
        /// <summary>
        /// The Task's number (used like a unique identifier).
        /// </summary>
        public int TaskNumber;

        /// <summary>
        /// Amount of time spent so far, in seconds.
        /// </summary>
        public long TimeSpentInSeconds;

        public TaskTracker( int taskNumber )
        {
            TimeSpentInSeconds = 0;
            TaskNumber = taskNumber;
        }

        /// <summary>
        /// Adds a given amount of seconds to this Task's time spent.
        /// </summary>
        /// <param name="seconds">A given amount of seconds.</param>
        /// <returns></returns>
        public long AddSeconds( int seconds )
        {
            TimeSpentInSeconds += seconds;

            return TimeSpentInSeconds;
        }
    }
}
