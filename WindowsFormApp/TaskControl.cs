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

        /// <summary>
        /// Updates the Task's time label according to the given time spent in seconds.
        /// </summary>
        /// <param name="timeSpentInSeconds">A given amount of time spent in seconds.</param>
        public void UpdateTimeLabel( long timeSpentInSeconds )
        {
            TimeSpan time = TimeSpan
                .FromSeconds( timeSpentInSeconds );

            TaskTime.Text = time.ToString( @"hh\:mm\:ss" );
        }

        /// <summary>
        /// Adjust Panel to match current controls.
        /// Should only be called after all (inner) controls are built.
        /// </summary>
        public void AdjustPanel()
        {
            // Update Panel with inner contents
            TaskPanel.Size = new Size(
                RemoveButton.Location.X + RemoveButton.Size.Width,
                // The +1 is for an extra pixel to accomodate the Panel margin
                RemoveButton.Size.Height + 1 );
            
            TaskPanel.Controls.Add( TaskDescription );
            TaskPanel.Controls.Add( TaskTime );
            TaskPanel.Controls.Add( ToggleButton );
            TaskPanel.Controls.Add( RemoveButton );
        }
    }
}
