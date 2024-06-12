namespace Forms
{
    partial class Thread2
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
            this.Resume2 = new System.Windows.Forms.Button();
            this.Pause2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Resume2
            // 
            this.Resume2.Location = new System.Drawing.Point(158, 237);
            this.Resume2.Name = "Resume2";
            this.Resume2.Size = new System.Drawing.Size(93, 33);
            this.Resume2.TabIndex = 1;
            this.Resume2.Text = "Resume";
            this.Resume2.UseVisualStyleBackColor = true;
            this.Resume2.Click += new System.EventHandler(this.Resume2_Click);
            // 
            // Pause2
            // 
            this.Pause2.Location = new System.Drawing.Point(34, 237);
            this.Pause2.Name = "Pause2";
            this.Pause2.Size = new System.Drawing.Size(93, 33);
            this.Pause2.TabIndex = 2;
            this.Pause2.Text = "Pause";
            this.Pause2.UseVisualStyleBackColor = true;
            this.Pause2.Click += new System.EventHandler(this.Pause2_Click);
            // 
            // Thread2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 276);
            this.Controls.Add(this.Resume2);
            this.Controls.Add(this.Pause2);
            this.Name = "Thread2";
            this.Text = "Thread 2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Thread2_FormClosed);
            this.Load += new System.EventHandler(this.Thread2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Resume2;
        private System.Windows.Forms.Button Pause2;
    }
}