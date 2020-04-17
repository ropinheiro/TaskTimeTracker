using System;
using System.Windows.Forms;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// The part responsible for the Task's tracking logic.
    /// Should NOT have anything regarding e.g. UI rendering.
    /// </summary>
    public class TaskTracker
    {
        // TODO: move Task UI stuff to TaskControl.

        public long TimeSpentInSeconds;
        public int TaskNumber;

        public Panel TaskPanel;
        public TextBox TaskDescription;
        public TextBox TaskTime;
        public Button ToggleButton;
        public Button RemoveButton;

        public TaskTracker()
        {
            TimeSpentInSeconds = 0;
            TaskNumber = 1;
        }

        public void AddSeconds( int seconds )
        {
            TimeSpentInSeconds += seconds;

            TimeSpan time = TimeSpan
                .FromSeconds( TimeSpentInSeconds );

            TaskTime.Text = time.ToString( @"hh\:mm\:ss" );
        }

        public override string ToString()
        {
            return $"Task {TaskNumber} | {TaskDescription.Text}";
        }
    }
}
