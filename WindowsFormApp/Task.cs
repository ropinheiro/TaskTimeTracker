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

        #region Task fields

        /// <summary>
        /// The Task's number (used like a unique identifier).
        /// </summary>
        public int TaskNumber
        {
            get
            {
                return _TaskNumber;
            }
            set
            {
                _TaskNumber = value;
                // TODO: update the Task Panel's position accordingly
            }
        }
        private int _TaskNumber;

        #endregion Task fields


        public Task( int taskNumber )
        {
            TaskNumber = taskNumber;
            Tracker = new TaskTracker();
            UI = new TaskControl();
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
            return $"Task {TaskNumber} | {UI.TaskDescription.Text}";
        }
    }
}
