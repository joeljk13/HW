namespace HW
{
    partial class AssignmentPrompt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssignmentPrompt));
            this.AssignmentLabel = new System.Windows.Forms.Label();
            this.AssignBox = new System.Windows.Forms.TextBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.DueLabel = new System.Windows.Forms.Label();
            this.DueOptions = new System.Windows.Forms.ComboBox();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.TimeEstBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // AssignmentLabel
            // 
            this.AssignmentLabel.AutoSize = true;
            this.AssignmentLabel.Location = new System.Drawing.Point(12, 15);
            this.AssignmentLabel.Name = "AssignmentLabel";
            this.AssignmentLabel.Size = new System.Drawing.Size(64, 13);
            this.AssignmentLabel.TabIndex = 5;
            this.AssignmentLabel.Text = "Assignment:";
            // 
            // AssignBox
            // 
            this.AssignBox.Location = new System.Drawing.Point(92, 12);
            this.AssignBox.MaxLength = 64;
            this.AssignBox.Name = "AssignBox";
            this.AssignBox.Size = new System.Drawing.Size(140, 20);
            this.AssignBox.TabIndex = 1;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Location = new System.Drawing.Point(56, 93);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(85, 30);
            this.ButtonOK.TabIndex = 3;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(147, 93);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(85, 30);
            this.ButtonCancel.TabIndex = 4;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // DueLabel
            // 
            this.DueLabel.AutoSize = true;
            this.DueLabel.Location = new System.Drawing.Point(12, 41);
            this.DueLabel.Name = "DueLabel";
            this.DueLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DueLabel.Size = new System.Drawing.Size(30, 13);
            this.DueLabel.TabIndex = 6;
            this.DueLabel.Text = "Due:";
            // 
            // DueOptions
            // 
            this.DueOptions.FormattingEnabled = true;
            this.DueOptions.Items.AddRange(new object[] {
            "Tomorrow",
            "In Two Days",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday"});
            this.DueOptions.Location = new System.Drawing.Point(92, 38);
            this.DueOptions.Name = "DueOptions";
            this.DueOptions.Size = new System.Drawing.Size(140, 21);
            this.DueOptions.TabIndex = 2;
            this.DueOptions.Text = "Tomorrow";
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Location = new System.Drawing.Point(12, 68);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(75, 13);
            this.TimeLabel.TabIndex = 8;
            this.TimeLabel.Text = "Time estimate:";
            // 
            // TimeEstBox
            // 
            this.TimeEstBox.FormattingEnabled = true;
            this.TimeEstBox.Items.AddRange(new object[] {
            "5 minutes",
            "10 minutes",
            "15 minutes",
            "20 minutes",
            "30 minutes",
            "45 minutes",
            "1 hour",
            "1.5 hours",
            "2 hours",
            "2.5 hours",
            "3+ hours"});
            this.TimeEstBox.Location = new System.Drawing.Point(92, 66);
            this.TimeEstBox.Name = "TimeEstBox";
            this.TimeEstBox.Size = new System.Drawing.Size(140, 21);
            this.TimeEstBox.TabIndex = 9;
            // 
            // AssignmentPrompt
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(269, 129);
            this.Controls.Add(this.TimeEstBox);
            this.Controls.Add(this.TimeLabel);
            this.Controls.Add(this.DueOptions);
            this.Controls.Add(this.DueLabel);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.AssignBox);
            this.Controls.Add(this.AssignmentLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AssignmentPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Assignment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IsClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AssignmentLabel;
        private System.Windows.Forms.TextBox AssignBox;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Label DueLabel;
        private System.Windows.Forms.ComboBox DueOptions;
        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.ComboBox TimeEstBox;
    }
}