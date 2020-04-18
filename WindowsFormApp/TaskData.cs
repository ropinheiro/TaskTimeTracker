using System.Collections.Generic;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// Task Data structure to read / write to a file in JSON format.
    /// </summary>
    public class TaskData
    {
        public readonly int TaskNumber;
        public readonly string TaskDescription;
        public readonly long TimeSpentInSeconds;

        public TaskData( Task task )
        {
            TaskNumber = task.TaskNumber;
            TaskDescription = task.UI.TaskDescription.Text;
            TimeSpentInSeconds = task.Tracker.TimeSpentInSeconds;
        }

        /// <summary>
        /// Convert a Task list to an array of Task Data.
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
