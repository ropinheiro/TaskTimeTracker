using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TEJ.TaskTimeTrackerApp
{
    public partial class TaskTimeTracker : Form
    {
        public List<Task> Tasks = new List<Task>();

        public Task CurrentTask;

        public TaskTimeTracker()
        {
            InitializeComponent();

            InitTasks();
        }

        private void btnStopStart_Click( object sender, EventArgs e )
        {
            Button clickedButton = ( (Button)sender );

            if ( tmrTaskTimer.Enabled )
            {
                CurrentTask.Tracker.TaskTime.BackColor = Color.Khaki;

                if ( IsSameTask( clickedButton ) )
                {
                    tmrTaskTimer.Stop();
                }
                else
                {
                    SetCurrentTaskByClickedButton( clickedButton );
                }
            }
            else
            {
                SetCurrentTaskByClickedButton( clickedButton );
                tmrTaskTimer.Start();
            }
        }

        private bool IsSameTask( Button clickedButton )
        {
            if ( CurrentTask == null )
                return false;

            return clickedButton == CurrentTask.Tracker.ToggleButton;
        }

        private void SetCurrentTaskByClickedButton( Button clickedButton )
        {
            CurrentTask = GetTaskByClickedButton( clickedButton );
            CurrentTask.Tracker.TaskTime.BackColor = Color.LawnGreen;
        }

        private Task GetTaskByClickedButton( Button clickedButton )
        {
            foreach ( Task task in Tasks )
            {
                if ( clickedButton == task.Tracker.ToggleButton
                    || clickedButton == task.Tracker.RemoveButton )
                {
                    return task;
                }
            }

            throw new Exception( "This button is not associated to a Task!" );
        }

        private void InitTasks()
        {
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

        private void TaskTimerEventProcessor( object sender, EventArgs e )
        {
            CurrentTask.Tracker.AddSeconds( 1 );
        }

        private void btnRemoveTask_Click( object sender, EventArgs e )
        {
            Task taskToRemove =
                GetTaskByClickedButton( (Button)sender );

            // TODO: move the limit elsewhere (e.g. contants? Configuration?)
            DialogResult confirmResult = MessageBox.Show(
                $"Remove task '{taskToRemove.Tracker.TaskDescription.Text}'?",
                "Are you sure?",
                MessageBoxButtons.YesNo );

            if ( confirmResult == DialogResult.Yes )
            {
                RemoveTask( taskToRemove );
            }
        }

        private void RemoveTask( Task taskToRemove )
        {
            // Adjust positions of all tasks coming after the
            // task to be removed (move them up one position).
            for ( int i = Tasks.Count - 1; i > taskToRemove.Tracker.TaskNumber - 1; i-- )
            {
                Tasks[ i ].Tracker.TaskNumber = Tasks[ i - 1 ].Tracker.TaskNumber;
                Tasks[ i ].Tracker.TaskPanel.Location =
                    Tasks[ i - 1 ].Tracker.TaskPanel.Location;
            }

            Controls.Remove( taskToRemove.Tracker.TaskPanel );
            Tasks.Remove( taskToRemove );

            AdjustFooter();
        }

        private void btnAddTask_Click( object sender, EventArgs e )
        {
            // TODO: move the limit elsewhere (e.g. contants? Configuration?)
            if ( Tasks != null && Tasks.Count >= 15 )
            {
                MessageBox.Show( "Too many tasks!", "Not allowed" );
            }
            else
            {
                AddNewTask( string.Empty );
            }
        }

        private void AddNewTask( string description )
        {
            // Base task
            TaskTracker newTask = new TaskTracker()
            {
                TaskNumber = GetNextTaskNumber()
            };
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

            newTask.TaskPanel = new Panel
            {
                Location = new Point( leftmostCoordinate, topCoordinate )
            };

            // Reset coordinates values as inner Panel's controls follow a local referential
            leftmostCoordinate = 0;
            topCoordinate = 0;

            // Construct Task Description control
            newTask.TaskDescription = new TextBox
            {
                Location = new Point( leftmostCoordinate, topCoordinate ),
                Size = new Size( descriptionWidth, lineHeight ),
                Text = description,
                TabIndex = ++tabIndex
            };

            // Construct Task Time control
            int taskTimeLeftCoordinate =
                leftmostCoordinate + descriptionWidth + horizontalSpace;

            newTask.TaskTime = new TextBox
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

            newTask.ToggleButton = new Button
            {
                Location = new Point( buttonLeftCoordinate, topCoordinate ),
                Size = new Size( buttonWidth, lineHeight ),
                TabIndex = ++tabIndex,
                Text = "GO",
                UseVisualStyleBackColor = true
            };
            newTask.ToggleButton.Click +=
                new EventHandler( btnStopStart_Click );

            // Construct Remove Button control
            int removeButtonLeftCoordinate =
                buttonLeftCoordinate + buttonWidth + horizontalSpace;

            newTask.RemoveButton = new Button
            {
                Location = new Point( removeButtonLeftCoordinate, topCoordinate ),
                Size = new Size( removeButtonWidth, lineHeight ),
                TabIndex = ++tabIndex,
                Text = "DEL",
                UseVisualStyleBackColor = true
            };
            newTask.RemoveButton.Click +=
                new EventHandler( btnRemoveTask_Click );

            // Update Panel with inner contents
            newTask.TaskPanel.Size = new Size(
                newTask.RemoveButton.Location.X + newTask.RemoveButton.Size.Width,
                newTask.RemoveButton.Size.Height + 1 );
            newTask.TaskPanel.Controls.Add( newTask.TaskDescription );
            newTask.TaskPanel.Controls.Add( newTask.TaskTime );
            newTask.TaskPanel.Controls.Add( newTask.ToggleButton );
            newTask.TaskPanel.Controls.Add( newTask.RemoveButton );

            // Add new Panel to the Form and Task to the list
            Controls.Add( newTask.TaskPanel );
            Tasks.Add( new Task() { Tracker = newTask } );

            AdjustFooter();
        }

        private int GetNextTaskNumber()
        {
            if ( Tasks == null || Tasks.Count == 0 )
                return 1;

            return Tasks.Last().Tracker.TaskNumber + 1;
        }

        private int GetCurrentTabIndex()
        {
            if ( Tasks == null || Tasks.Count == 0 )
                return 0;

            return Tasks.Last().Tracker.RemoveButton.TabIndex;
        }

        private void AdjustFooter()
        {
            // TODO: move the magic numbers to constants

            Task lastTask = Tasks.Last();

            // Adjust Add Button to be after last Task
            int titleHeight =
                RectangleToScreen( ClientRectangle ).Top - Top;

            btnAddTask.Top =
                lastTask.Tracker.TaskPanel.Top
                + lastTask.Tracker.TaskPanel.Height
                + 10;

            // Adjust Form's height to the contents
            Height = titleHeight
                + btnAddTask.Top
                + btnAddTask.Height
                + 20;
        }
    }
}