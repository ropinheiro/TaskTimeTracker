namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// Abstractly describes a Task.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// The part responsible for the Task's UI rendering.
        /// </summary>
        public TaskControl UI;

        /// <summary>
        /// The part responsible for the Task's tracking logic.
        /// </summary>
        public TaskTracker Tracker;

        public Task( int taskNumber )
        {
            UI = new TaskControl();
            Tracker = new TaskTracker( taskNumber );
        }

        /// <summary>
        /// Adds a given amount of seconds to this Task's time spent
        /// and updates the UI accordingly.
        /// </summary>
        /// <param name="seconds">A given amount of seconds.</param>
        public void AddSeconds( int seconds )
        {
            long timeSpentInSeconds =
                Tracker.AddSeconds( seconds );

            UI.UpdateTimeLabel( timeSpentInSeconds );
        }

        /// <summary>
        /// Outputs a string shortly descibring the Task.
        /// </summary>
        /// <returns>A string that describes the Task.</returns>
        public override string ToString()
        {
            return $"Task {Tracker.TaskNumber} | {UI.TaskDescription.Text}";
        }
    }
}
