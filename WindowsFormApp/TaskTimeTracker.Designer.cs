namespace TEJ.TaskTimeTrackerApp
{
    partial class TaskTimeTracker
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTask = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.tmrTaskTimer = new System.Windows.Forms.Timer(this.components);
            this.btnAddTask = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTask
            // 
            this.lblTask.AutoSize = true;
            this.lblTask.Location = new System.Drawing.Point(12, 16);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(49, 25);
            this.lblTask.TabIndex = 0;
            this.lblTask.Text = "Task:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(460, 16);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(54, 25);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "Time:";
            // 
            // tmrTaskTimer
            // 
            this.tmrTaskTimer.Interval = 1000;
            this.tmrTaskTimer.Tick += new System.EventHandler(this.TaskTimerEventProcessor);
            // 
            // btnAddTask
            // 
            this.btnAddTask.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAddTask.Location = new System.Drawing.Point(10, 100);
            this.btnAddTask.Name = "btnAddTask";
            this.btnAddTask.Size = new System.Drawing.Size(660, 50);
            this.btnAddTask.TabIndex = 1;
            this.btnAddTask.Text = "Add";
            this.btnAddTask.UseVisualStyleBackColor = true;
            this.btnAddTask.Click += new System.EventHandler(this.AddTask_Click);
            // 
            // TaskTimeTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 152);
            this.Controls.Add(this.btnAddTask);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblTask);
            this.Name = "TaskTimeTracker";
            this.Text = "Task Time Tracker @ Estamos Juntos 2020 - RMP";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrTaskTimer;
        private System.Windows.Forms.Button btnAddTask;
    }
}

