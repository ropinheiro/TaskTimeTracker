using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// Loads / saves Task list data in a file.
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Saves a given Task list to a file in JSON format.
        /// </summary>
        /// <param name="tasks"></param>
        public static void SaveData( List<Task> tasks )
        {
            TaskData[] data = TaskData.GetTaskDataFromTaskList( tasks );

            using ( StreamWriter file = File.CreateText( @"TaskData.json" ) )
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize( file, data );
            }
        }
    }
}
