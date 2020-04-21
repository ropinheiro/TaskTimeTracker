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
        /// Changing it affects the position of the Task's Panel in the UI.
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

                if ( UI != null )
                    UI.ConfigurePanelByTaskNumber( _TaskNumber );
            }
        }
        private int _TaskNumber;

        #endregion Task fields

        public Task( int number, string description, long timeSpentInSeconds, TaskTimeTracker form )
        {
            TaskNumber = number;

            Tracker = new TaskTracker( timeSpentInSeconds );

            UI = new TaskControl( number, description, timeSpentInSeconds, form );
        }

        /// <summary>
        /// Updates the UI with the current task's time spent.
        /// </summary>
        public void UpdateTimeLabel()
        {
            UI.UpdateTimeLabel( Tracker.TotalTimeSpentInSeconds );
        }

        /// <summary>
        /// React to the timer being started for this task.
        /// </summary>
        public void TimerStarted()
        {
            UI.TimerStarted();
            Tracker.TimerStarted();
        }

        /// <summary>
        /// React to the timer being stopped for this task.
        /// </summary>
        public void TimerStopped()
        {
            UI.TimerStopped();
            Tracker.TimerStopped();
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
