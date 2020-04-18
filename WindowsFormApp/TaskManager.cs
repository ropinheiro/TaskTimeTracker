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

        public void InitTasks()
        {
            // Intialize app with some tasks
            // TODO: should load them from the disk.
            List<string> taskDescriptions = new List<string>
            {
                "Task 1",
                "Task 2",
                "Task 3",
                "Task 4",
                "Task 5",
            };

            foreach ( string description in taskDescriptions )
                AddNewTask( description );
        }

        #region Add / Remove Tasks

        /// <summary>
        /// Adds a new Task with a given description to the end of the list.
        /// </summary>
        /// <param name="taskDescription">A given Task descripiton</param>
        public void AddNewTask( string taskDescription )
        {
            // Base task information
            Task newTask = new Task( GetNextTaskNumber() );
            int tabIndex = GetCurrentTabIndex();

            // TODO: move this somewhere (e.g. to constants?)
            int descriptionWidth = 450;
            int timeWidth = 95;
            int buttonWidth = 50;
            int removeButtonWidth = 50;
            int lineHeight = 30;
            int verticalSpace = 10;
            int horizontalSpace = 5;
            int leftmostCoordinate = 10;
            int topmostCoordinate = 50;

            // Construct Panel control
            int topCoordinate = topmostCoordinate +
                ( ( newTask.TaskNumber - 1 ) * ( lineHeight + verticalSpace ) );

            newTask.UI.TaskPanel = new Panel
            {
                Location = new Point( leftmostCoordinate, topCoordinate )
            };

            // Reset coordinates values as inner Panel's controls follow a local referential
            leftmostCoordinate = 0;
            topCoordinate = 0;

            // Construct Task Description control
            newTask.UI.TaskDescription = new TextBox
            {
                Location = new Point( leftmostCoordinate, topCoordinate ),
                Size = new Size( descriptionWidth, lineHeight ),
                Text = taskDescription,
                TabIndex = ++tabIndex
            };
            newTask.UI.TaskDescription.TextChanged +=
                new EventHandler( Form.AnyInputChanges );

            // Construct Task Time control
            int taskTimeLeftCoordinate =
                leftmostCoordinate + descriptionWidth + horizontalSpace;

            newTask.UI.TaskTime = new TextBox
            {
                ReadOnly = true,
                BackColor = Color.LightCoral,
                ForeColor = Color.Black,
                Location = new Point( taskTimeLeftCoordinate, topCoordinate ),
                Size = new Size( timeWidth, lineHeight ),
                TabIndex = ++tabIndex,
                Text = "00:00:00",
                TextAlign = HorizontalAlignment.Center
            };

            // Construct Task Button control
            int buttonLeftCoordinate =
                taskTimeLeftCoordinate + timeWidth + horizontalSpace;

            newTask.UI.ToggleButton = new Button
            {
                Location = new Point( buttonLeftCoordinate, topCoordinate ),
                Size = new Size( buttonWidth, lineHeight ),
                TabIndex = ++tabIndex,
                Text = "GO",
                UseVisualStyleBackColor = true
            };
            newTask.UI.ToggleButton.Click +=
                new EventHandler( Form.ToogleTimer_Click );

            // Construct Remove Button control
            int removeButtonLeftCoordinate =
                buttonLeftCoordinate + buttonWidth + horizontalSpace;

            newTask.UI.RemoveButton = new Button
            {
                Location = new Point( removeButtonLeftCoordinate, topCoordinate ),
                Size = new Size( removeButtonWidth, lineHeight ),
                TabIndex = ++tabIndex,
                Text = "DEL",
                UseVisualStyleBackColor = true
            };
            newTask.UI.RemoveButton.Click +=
                new EventHandler( Form.RemoveTask_Click );

            // Update Panel with inner contents
            newTask.UI.AdjustPanel();

            // Add new Task to the list
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
                Tasks[ i ].UI.TaskPanel.Location =
                    Tasks[ i - 1 ].UI.TaskPanel.Location;
            }

            Form.Controls.Remove( taskToRemove.UI.TaskPanel );
            Tasks.Remove( taskToRemove );

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

        /// <summary>
        /// Gets the current Tab Index.
        /// </summary>
        /// <returns>The current Tab Index.</returns>
        private int GetCurrentTabIndex()
        {
            if ( Tasks == null || Tasks.Count == 0 )
                return 0;

            return Tasks.Last().UI.RemoveButton.TabIndex;
        }

        #endregion Helpers
    }
}
