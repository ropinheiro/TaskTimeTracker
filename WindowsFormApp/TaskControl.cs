using System;
using System.Drawing;
using System.Windows.Forms;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// The part responsible for the Task's UI rendering.
    /// Should NOT have anything regarding e.g. tracking logic.
    /// </summary>
    public class TaskControl
    {
        #region Controls

        /// <summary>
        /// A Panel to hold all Task-related controls.
        /// This way, we can do group actions in the task controls by manipulating the Panel.
        /// </summary>
        public Panel TaskPanel;

        /// <summary>
        /// A control to hold this Task's description.
        /// </summary>
        public TextBox TaskDescription;

        /// <summary>
        /// A control to hold this Task's time spent so far.
        /// </summary>
        public TextBox TaskTime;

        /// <summary>
        /// A control for the button that toggles the Timer for this Task.
        /// </summary>
        public Button ToggleButton;

        /// <summary>
        /// A control for the button that removes this Task.
        /// </summary>
        public Button RemoveButton;

        #endregion Controls

        public TaskControl( int taskNumber, string taskDescription, TaskTimeTracker form )
        {
            // TODO: move this somewhere (e.g. to constants?)
            int descriptionWidth = 450;
            int timeWidth = 95;
            int buttonWidth = 50;
            int removeButtonWidth = 50;
            int lineHeight = 30;
            int horizontalSpace = 5;

            // ----------------------------------------------------------------
            // Construct Panel control
            // ----------------------------------------------------------------
            ConfigurePanelByTaskNumber( taskNumber );

            // ----------------------------------------------------------------
            // Construct Task Description control
            // ----------------------------------------------------------------
            TaskDescription = new TextBox
            {
                Location = new Point( 0, 0 ),
                Size = new Size( descriptionWidth, lineHeight ),
                Text = taskDescription,
                TabIndex = 0
            };
            TaskDescription.TextChanged +=
                new EventHandler( form.AnyInputChanges );

            // ----------------------------------------------------------------
            // Construct Task Time control
            // ----------------------------------------------------------------
            int taskTimeLeftCoordinate =
                descriptionWidth + horizontalSpace;

            TaskTime = new TextBox
            {
                ReadOnly = true,
                BackColor = Color.LightCoral,
                ForeColor = Color.Black,
                Location = new Point( taskTimeLeftCoordinate, 0 ),
                Size = new Size( timeWidth, lineHeight ),
                TabIndex = 1,
                Text = GetFormattedTime( 0 ),
                TextAlign = HorizontalAlignment.Center
            };

            // ----------------------------------------------------------------
            // Construct Task Button control
            // ----------------------------------------------------------------
            int buttonLeftCoordinate =
                taskTimeLeftCoordinate + timeWidth + horizontalSpace;

            ToggleButton = new Button
            {
                Location = new Point( buttonLeftCoordinate, 0 ),
                Size = new Size( buttonWidth, lineHeight ),
                TabIndex = 2,
                Text = "GO",
                UseVisualStyleBackColor = true
            };
            ToggleButton.Click +=
                new EventHandler( form.ToogleTimer_Click );

            // ----------------------------------------------------------------
            // Construct Remove Button control
            // ----------------------------------------------------------------
            int removeButtonLeftCoordinate =
                buttonLeftCoordinate + buttonWidth + horizontalSpace;

            RemoveButton = new Button
            {
                Location = new Point( removeButtonLeftCoordinate, 0 ),
                Size = new Size( removeButtonWidth, lineHeight ),
                TabIndex = 3,
                Text = "DEL",
                UseVisualStyleBackColor = true
            };
            RemoveButton.Click +=
                new EventHandler( form.RemoveTask_Click );

            // ----------------------------------------------------------------
            // Update Panel with inner contents
            // ----------------------------------------------------------------
            AdjustPanel();
        }

        /// <summary>
        /// Updates the Task's time label according to the given time spent in seconds.
        /// </summary>
        /// <param name="timeSpentInSeconds">A given amount of time spent in seconds.</param>
        public void UpdateTimeLabel( long timeSpentInSeconds )
        {
            TaskTime.Text = GetFormattedTime( timeSpentInSeconds );
        }

        /// <summary>
        /// Returns a formatted string with the given time spent in seconds.
        /// </summary>
        /// <param name="timeSpentInSeconds">A given amount of time spent in seconds.</param>
        private string GetFormattedTime( long timeSpentInSeconds )
        {
            TimeSpan time = TimeSpan
                .FromSeconds( timeSpentInSeconds );

            return time.ToString( @"hh\:mm\:ss" );
        }

        /// <summary>
        /// Adjust Panel to match current controls.
        /// Should only be called after all (inner) controls are built.
        /// </summary>
        public void AdjustPanel()
        {
            TaskPanel.Size = new Size(
                RemoveButton.Location.X + RemoveButton.Size.Width,
                // The +1 is for an extra pixel to accomodate the Panel margin
                RemoveButton.Size.Height + 1 );

            TaskPanel.Controls.Clear();
            TaskPanel.Controls.Add( TaskDescription );
            TaskPanel.Controls.Add( TaskTime );
            TaskPanel.Controls.Add( ToggleButton );
            TaskPanel.Controls.Add( RemoveButton );
        }

        /// <summary>
        /// Creates or updates the Panel's position and tab index according
        /// to the given Task number.
        /// </summary>
        /// <param name="taskNumber">A given Task number.</param>
        public void ConfigurePanelByTaskNumber( int taskNumber )
        {
            // TODO: move this somewhere (e.g. to constants?)
            int lineHeight = 30;
            int verticalSpace = 10;
            int leftmostCoordinate = 10;
            int topmostCoordinate = 50;

            int topCoordinate = topmostCoordinate +
                ( ( taskNumber - 1 ) * ( lineHeight + verticalSpace ) );

            if ( TaskPanel == null )
                TaskPanel = new Panel();

            TaskPanel.Location = new Point( leftmostCoordinate, topCoordinate );
            TaskPanel.TabIndex = taskNumber;
        }
    }
}
