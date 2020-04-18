using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// Manages the task list and actions that must be performed on it.
    /// </summary>
    public class TaskManager
    {
        /// <summary>
        /// The complete list of Tasks
        /// </summary>
        public List<Task> Tasks = new List<Task>();

        /// <summary>
        /// The currently selected task. Only this task will have its time being tracked.
        /// </summary>
        public Task CurrentTask;

        /// <summary>
        /// The UI where the action occurs. Contains not only the main controls,
        /// but also events thatn can be subscribed.
        /// </summary>
        private readonly TaskTimeTracker Form;

        public TaskManager( TaskTimeTracker form )
        {

            Form = form;
        }

        /// <summary>
        /// Load previous tasks stored in a file.
        /// </summary>
        public void LoadTasks()
        {
            List<TaskData> rawData = FileManager.LoadData();

            foreach ( TaskData data in rawData )
            {
                AddNewTask( data.TaskDescription, data.TimeSpentInSeconds );
            }
        }

        #region Add / Remove Tasks

        /// <summary>
        /// Adds a new empty task, without description nor elapsed time.
        /// </summary>
        public void AddNewEmptyTask()
        {
            AddNewTask( string.Empty, 0 );
        }

        /// <summary>
        /// Adds a new Task with a given description to the end of the list.
        /// </summary>
        /// <param name="taskDescription">A given Task descripiton</param>
        /// <param name="timeSpentInSeconds">A given Time spent in seconds</param>
        public void AddNewTask( string taskDescription, long timeSpentInSeconds )
        {
            Task newTask =
                new Task( GetNextTaskNumber(), taskDescription, timeSpentInSeconds, Form );

            Tasks.Add( newTask );

            // Update UI
            Form.Controls.Add( newTask.UI.TaskPanel );
            Form.AdjustFooter();
        }

        /// <summary>
        /// Removes a given Task from the list and the UI.
        /// </summary>
        /// <param name="taskToRemove">A given Task to remove.</param>
        public void RemoveTask( Task taskToRemove )
        {
            // Adjust positions of all tasks coming after the
            // task to be removed (move them up one position).
            for ( int i = Tasks.Count - 1; i > taskToRemove.TaskNumber - 1; i-- )
            {
                Tasks[ i ].TaskNumber = Tasks[ i - 1 ].TaskNumber;
            }

            // Remove the Task from the Task list and from the UI.
            Tasks.Remove( taskToRemove );
            Form.Controls.Remove( taskToRemove.UI.TaskPanel );

            // Shrink the Form now that we have one less Task.
            Form.AdjustFooter();
        }

        #endregion Add / Remove Tasks

        #region Helpers

        /// <summary>
        /// Check if there are any Tasks in the list.
        /// </summary>
        /// <returns>True if there are, false otherwise.</returns>
        public bool HasTasks()
        {
            if ( Tasks == null )
                return false;

            if ( Tasks.Count == 0 )
                return false;

            return true;
        }

        /// <summary>
        /// Returns the last Task in the list.
        /// </summary>
        /// <returns>A Task if list is non-empty. If it is empty, returns null.</returns>
        public Task LastTask()
        {
            if ( HasTasks() )
                return Tasks.Last();
            else
                return null;
        }
        
        /// <summary>
        /// Gets the last vertical pixel occupied by the Task list.
        /// Useful if you need to know from which pixel in the Form you can render other stuff.
        /// </summary>
        /// <returns>A pixel referring to a Y coordinate (an integer number)</returns>
        public int LastVerticalPixel()
        {
            // TODO: move to a constant.
            int topmostCoordinate = 50;

            Task lastTask = LastTask();

            if ( lastTask == null )
                return topmostCoordinate;
            else
                return
                    lastTask.UI.TaskPanel.Top
                    + lastTask.UI.TaskPanel.Height;
        }

        /// <summary>
        /// Checks if a given clicked Button belongs to the Current Task
        /// </summary>
        /// <param name="clickedButton"></param>
        /// <returns>True if the given clicked Button belongs to the Current Task. False otherwise.</returns>
        public bool ButtonBelongsToTheCurrentTask( Button clickedButton )
        {
            if ( CurrentTask == null )
                return false;

            return clickedButton == CurrentTask.UI.ToggleButton;
        }

        /// <summary>
        /// Finds the Task to which the given clicked Button belongs.
        /// </summary>
        /// <param name="clickedButton">A given clicked Button.</param>
        /// <returns>A Task.</returns>
        /// <exception cref="System.Exception">Throw when no Task is found.</exception>
        public Task GetTaskByClickedButton( Button clickedButton )
        {
            foreach ( Task task in Tasks )
            {
                if ( clickedButton == task.UI.ToggleButton
                    || clickedButton == task.UI.RemoveButton )
                {
                    return task;
                }
            }

            throw new Exception( "This button is not associated to a Task!" );
        }

        /// <summary>
        /// Sets the Current Task according to the clicked Button.
        /// </summary>
        /// <param name="clickedButton">A given clicked Button.</param>
        public void SetCurrentTaskByClickedButton( Button clickedButton )
        {
            CurrentTask = GetTaskByClickedButton( clickedButton );
            CurrentTask.UI.TaskTime.BackColor = Color.LawnGreen;
        }

        /// <summary>
        /// Adds the given number of seconds to the Current Task
        /// </summary>
        /// <param name="seconds">A given number of seconds</param>
        public void AddSeconds( int seconds )
        {
            CurrentTask.AddSeconds( seconds );
        }

        public bool HasTooManyTasks()
        {
            if ( !HasTasks() )
                return false;

            // TODO: move the limit elsewhere (e.g. contants? Configuration?)
            return Tasks.Count >= 15;
        }

        /// <summary>
        /// Gets the next Task Number.
        /// </summary>
        /// <returns>A Task Number.</returns>
        private int GetNextTaskNumber()
        {
            if ( Tasks == null || Tasks.Count == 0 )
                return 1;

            return Tasks.Last().TaskNumber + 1;
        }

        #endregion Helpers
    }
}