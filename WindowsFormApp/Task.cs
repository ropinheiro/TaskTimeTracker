using System.Linq;
using System.Windows.Forms;

namespace TEJ.TaskTimeTrackerApp
{
    /// <summary>
    /// Abstractly describes a Task.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// The part responsible for the Task's UI rendering.
        /// </summary>
        public TaskControl UI;

        /// <summary>
        /// The part responsible for the Task's tracking logic.
        /// </summary>
        public TaskTracker Tracker;

        #region Task fields

        /// <summary>
        /// The Task's number (used like a unique identifier).
        /// Changing it affects the position of the Task's Panel in the UI.
        /// </summary>
        public int TaskNumber
        {
            get
            {
                return _TaskNumber;
            }
            set
            {
                _TaskNumber = value;

                if ( UI != null )
                    UI.ConfigurePanelByTaskNumber( _TaskNumber );
            }
        }
        private int _TaskNumber;

        /// <summary>
        /// Tells if tiem is being tracked for this Task, or not.
        /// </summary>
        private bool IsTrackingTime = false;

        #endregion Task fields

        public Task( int number, string description, long timeSpentInSeconds, TaskTimeTracker form )
        {
            TaskNumber = number;

            Tracker = new TaskTracker( timeSpentInSeconds );

            UI = new TaskControl( number, description, timeSpentInSeconds, form );
        }

        /// <summary>
        /// Updates the UI with the current task's time spent.
        /// </summary>
        public void UpdateTimeInput()
        {
            UI.UpdateTimeLabel( Tracker.TotalTimeSpentInSeconds );
        }

        /// <summary>
        /// Updates the Task's time to a specified value.
        /// </summary>
        /// <param name="newValueAsText"></param>
        public void UpdateTimeTo( string newValueAsText )
        {
            int newSeconds = ConvertToSeconds( newValueAsText );

            if ( newSeconds >= 0 )
            {
                Tracker.TimePreviouslySpentInSeconds = newSeconds;
            }
            else
            {
                // TODO: TaskControl should be responsible for all UI, including error messages
                MessageBox.Show(
                    "Value must: "
                    + System.Environment.NewLine
                    + "1. Be a number > 0 for the seconds (e.g. 300 = 00:05:00)"
                    + System.Environment.NewLine
                    + "2. Or follow format mm:ss (e.g. 12:34 = 00:12:34)"
                    + System.Environment.NewLine
                    + "3. Or follow format hh:mm:ss (from 00:00:00 to 23:59:59)."
                    + System.Environment.NewLine,
                    "Invalid time format!" );
            }

            // No matter what happens, always update the Time input so that
            // either the new (if valid) value is wrote, or the previous
            // value (when the new is invalid) is restored.
            UpdateTimeInput();
        }

        /// <summary>
        /// Converts a given text value to seconds according to the accepted formats:
        /// 1. Any integer number - it will be the number of seconds.
        /// 2. Format mm:ss | mm and ss are in the [ 0, 59 ] interval.
        /// 3. Format hh:mm:ss | hh is in the [ 0, 23 ] interval and the remaining is like 2.
        /// </summary>
        /// <param name="valueAsText"></param>
        /// <returns></returns>
        public int ConvertToSeconds( string valueAsText )
        {
            /// TODO: maybe use regular expression instead of those if's?

            // Empty string? Return zero (= reset value)
            if ( string.IsNullOrWhiteSpace( valueAsText ) )
                return 0;

            // Plain integer? Then it's the number of seconds, return it.
            if ( uint.TryParse( valueAsText, out uint number ) )
                return (int)number;

            // A hack to make the conversion easier!
            // Just add zero hours if we clearly see that they're not there!
            // This way we always check for hh:mm:ss format.
            if ( valueAsText.Length == 5 )
                valueAsText = "00:" + valueAsText;

            string[] parts = valueAsText.Split( ':' );
            if ( parts.Count() == 3 )
            {
                if ( !int.TryParse( parts[ 2 ], out int seconds ) )
                    return -1;
                if ( seconds < 0 || seconds > 59 )
                    return -1;

                if ( !int.TryParse( parts[ 1 ], out int minutes ) )
                    return -1;
                if ( minutes < 0 || minutes > 59 )
                    return -1;

                if ( !int.TryParse( parts[ 0 ], out int hours ) )
                    return -1;
                if ( hours < 0 || hours > 23 )
                    return -1;

                return seconds + ( minutes * 60 ) + ( hours * 60 * 60 );
            }

            return -1;
        }

        /// <summary>
        /// React to the timer being started for this task.
        /// </summary>
        public void TimerStarted()
        {
            IsTrackingTime = true;
            UI.TimerStarted();
            Tracker.TimerStarted();
        }

        /// <summary>
        /// React to the timer being stopped for this task.
        /// </summary>
        public void TimerStopped()
        {
            IsTrackingTime = false;
            UI.TimerStopped();
            Tracker.TimerStopped();
        }

        /// <summary>
        /// Outputs a string shortly descibring the Task.
        /// </summary>
        /// <returns>A string that describes the Task.</returns>
        public override string ToString()
        {
            return $"Task {TaskNumber} | {UI.TaskDescription.Text}";
        }
    }
}
