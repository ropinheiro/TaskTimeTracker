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
    }
}
