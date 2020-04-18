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
        /// The filename to be used to save the data.
        /// </summary>
        private const string FileName = @"TaskData.json";

        /// <summary>
        /// Saves a given Task list to a file in JSON format.
        /// </summary>
        /// <param name="tasks"></param>
        public static void SaveData( List<Task> tasks )
        {
            TaskData[] data = TaskData.GetTaskDataFromTaskList( tasks );

            using ( StreamWriter file = File.CreateText( FileName ) )
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize( file, data );
            }
        }

        /// <summary>
        /// Loads the current stored Tasks from a file in JSON format.
        /// </summary>
        /// <returns>A list of Task Data.</returns>
        public static List<TaskData> LoadData()
        {
            // TODO: what if file doesn't exist or is corrupted?
            //       must add some sort of fallback to create an empty list.

            List<TaskData> data;
            using ( FileStream fileStream = new FileStream( FileName, FileMode.Open ) )
            {
                JsonSerializer serializer = new JsonSerializer();
                using ( StreamReader streamReader = new StreamReader( fileStream ) )
                {
                    data = (List<TaskData>)serializer.Deserialize( streamReader, typeof( List<TaskData> ) );
                }
            }

            // TODO: sanitize data? e.g:
            //       - Renumbering tasks to prevent holes caused by user editions.
            //       - Fixing invalid elapsed times, reseting them to zero.     

            return data;
        }
    }
}
