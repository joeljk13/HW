namespace HW
{
    partial class HW
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HW));
            this.SuspendLayout();
            // 
            // HW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 62);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(250, 60);
            this.Name = "HW";
            this.Text = "Home Work";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HW_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HW_FormClosed);
            this.Load += new System.EventHandler(this.HW_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FindKeyCombos);
            this.ResumeLayout(false);

        }

        #endregion

    }
}