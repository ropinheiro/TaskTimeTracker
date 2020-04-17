using System;
using System.Collections.Generic;
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

        public TaskTimeTracker()
        {
            InitializeComponent();

            Manager = new TaskManager( this );
            Manager.InitTasks();
        }

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
                Manager.AddNewTask( string.Empty );
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
                $"Remove task '{taskToRemove.Tracker.TaskDescription.Text}'?",
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
                Manager.CurrentTask.Tracker.TaskTime.BackColor = Color.Khaki;

                if ( Manager.ButtonBelongsToTheCurrentTask( clickedButton ) )
                {
                    tmrTaskTimer.Stop();
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

        /// <summary>
        /// Executes when Timer ticks.
        /// </summary>
        /// <param name="sender">The Timer that ticked.</param>
        /// <param name="e">Event arguments (unused)</param>
        private void TaskTimerEventProcessor( object sender, EventArgs e )
        {
            Manager.AddSeconds( 1 );
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

            // Adjust Form's height to the contents
            Height = titleHeight
                + btnAddTask.Top
                + btnAddTask.Height
                + 20;
        }
    }
}