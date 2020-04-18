using System.Collections.Generic;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// Task Data structure to read / write to a file in JSON format.
    /// </summary>
    public class TaskData
    {
        public string TaskDescription { get; set; }

        public long TimeSpentInSeconds { get; set; }

        public TaskData() { /* Used when parsing from JSON */ }

        public TaskData( Task task )
        {
            TaskDescription = task.UI.TaskDescription.Text;
            TimeSpentInSeconds = task.Tracker.TimeSpentInSeconds;
        }

        /// <summary>
        /// Converts a Task list to an array of Task Data.
        /// </summary>
        /// <param name="tasks">A given Task list.</param>
        /// <returns>An array of Task Data.</returns>
        public static TaskData[] GetTaskDataFromTaskList( List<Task> tasks )
        {
            TaskData[] data = new TaskData[ tasks.Count ];

            for ( int i = 0; i < tasks.Count; i++ )
                data[ i ] = new TaskData( tasks[ i ] );

            return data;
        }
    }
}
