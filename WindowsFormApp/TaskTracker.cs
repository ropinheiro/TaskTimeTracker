namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// The part responsible for the Task's tracking logic.
    /// Should NOT have anything regarding e.g. UI rendering.
    /// </summary>
    public class TaskTracker
    {
        /// <summary>
        /// Amount of time spent so far, in seconds.
        /// </summary>
        public long TimeSpentInSeconds;

        public TaskTracker()
        {
            TimeSpentInSeconds = 0;
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
