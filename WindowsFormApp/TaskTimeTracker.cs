using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TEJ.TaskTimeTrackerApp
{
    public partial class TaskTimeTracker : Form
    {
        /// <summary>
        /// The Task Manager that will be responsible by
        /// handling the list of Tasks.
        /// </summary>
        private readonly TaskManager Manager;

        public bool DataIsDirty = false;

        public TaskTimeTracker()
        {
            InitializeComponent();

            Manager = new TaskManager( this );
            Manager.LoadTasks();

            tmrFileSaver.Start();
        }

        #region Button handlers

        /// <summary>
        /// Executes when user clicks the Add Task button.
        /// </summary>
        /// <param name="sender">The Button that was clicked.</param>
        /// <param name="e">Event arguments (unused)</param>
        private void AddTask_Click( object sender, EventArgs e )
        {
            // TODO: move the limit elsewhere (e.g. contants? Configuration?)
            if ( Manager.HasTooManyTasks() )
            {
                MessageBox.Show( "Too many tasks!", "Not allowed" );
            }
            else
            {
                Manager.AddNewEmptyTask();
            }
        }

        /// <summary>
        /// Executes when user clicks a Task's removal button.
        /// </summary>
        /// <param name="sender">The Button that was clicked.</param>
        /// <param name="e">Event arguments (unused)</param>
        public void RemoveTask_Click( object sender, EventArgs e )
        {
            Task taskToRemove =
                Manager.GetTaskByClickedButton( (Button)sender );

            // TODO: move the limit elsewhere (e.g. contants? Configuration?)
            DialogResult confirmResult = MessageBox.Show(
                $"Remove task '{taskToRemove.UI.TaskDescription.Text}'?",
                "Are you sure?",
                MessageBoxButtons.YesNo );

            if ( confirmResult == DialogResult.Yes )
            {
                Manager.RemoveTask( taskToRemove );
            }
        }

        /// <summary>
        /// Executes when user clicks a Task's timer toggle button.
        /// </summary>
        /// <param name="sender">The Button that was clicked.</param>
        /// <param name="e">Event arguments (unused)</param>
        public void ToogleTimer_Click( object sender, EventArgs e )
        {
            Button clickedButton = ( (Button)sender );

            if ( tmrTaskTimer.Enabled )
            {
                Manager.CurrentTask.UI.TaskTime.BackColor = Color.Khaki;

                if ( Manager.ButtonBelongsToTheCurrentTask( clickedButton ) )
                {
                    tmrTaskTimer.Stop();

                    // Do this when stopping the Task tracking timer to ensure
                    // that a last file save occurs with the current data.
                    SaveFile();
                }
                else
                {
                    Manager.SetCurrentTaskByClickedButton( clickedButton );
                }
            }
            else
            {
                Manager.SetCurrentTaskByClickedButton( clickedButton );
                tmrTaskTimer.Start();
            }
        }

        #endregion Button handlers

        #region Timer handlers

        /// <summary>
        /// Executes when Task Timer ticks.
        /// </summary>
        /// <param name="sender">The Timer that ticked.</param>
        /// <param name="e">Event arguments (unused)</param>
        private void TaskTimerEventProcessor( object sender, EventArgs e )
        {
            Manager.AddSeconds( 1 );
        }

        /// <summary>
        /// Executes when File Saver Timer ticks.
        /// </summary>
        /// <param name="sender">The Timer that ticked.</param>
        /// <param name="e">Event arguments (unused)</param>
        private void FileSaverEventProcessor( object sender, EventArgs e )
        {
            // Always save file if Task Timer is enabled.
            bool saveFile = tmrTaskTimer.Enabled;

            // If it is not enabled... save file if actual data is "dirty".
            if ( !saveFile && DataIsDirty )
                saveFile = true;

            if ( saveFile )
            {
                SaveFile();
            }
        }

        /// <summary>
        /// Execute a File saving
        /// </summary>
        private void SaveFile()
        {
            DataIsDirty = false;
            FileManager.SaveData( Manager.Tasks );
        }

        #endregion Timer handlers

        /// <summary>
        /// Executes when any user input changes.
        /// </summary>
        /// <param name="sender">The input control that changed.</param>
        /// <param name="e">Event arguments (unused)</param>
        public void AnyInputChanges( object sender, EventArgs e )
        {
            DataIsDirty = true;
        }


        /// <summary>
        /// Adjusts Add button and Form height according
        /// to the space occupied by the current Task list.
        /// </summary>
        public void AdjustFooter()
        {
            // TODO: move the magic numbers to constants

            // Adjust Add Button to be after last Task
            int titleHeight =
                RectangleToScreen( ClientRectangle ).Top - Top;

            btnAddTask.Top =
                Manager.LastVerticalPixel() + 10;
            btnAddTask.TabIndex = Manager.Tasks.Count() + 1;

            // Adjust Form's height to the contents
            Height = titleHeight
                + btnAddTask.Top
                + btnAddTask.Height
                + 20;
        }
    }
}