namespace Forms
{
    partial class Thread1
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
            this.Pause1 = new System.Windows.Forms.Button();
            this.Resume1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Pause1
            // 
            this.Pause1.Location = new System.Drawing.Point(34, 237);
            this.Pause1.Name = "Pause1";
            this.Pause1.Size = new System.Drawing.Size(93, 33);
            this.Pause1.TabIndex = 3;
            this.Pause1.Text = "Pause";
            this.Pause1.UseVisualStyleBackColor = true;
            this.Pause1.Click += new System.EventHandler(this.Pause1_Click);
            // 
            // Resume1
            // 
            this.Resume1.Location = new System.Drawing.Point(158, 237);
            this.Resume1.Name = "Resume1";
            this.Resume1.Size = new System.Drawing.Size(93, 33);
            this.Resume1.TabIndex = 1;
            this.Resume1.Text = "Resume";
            this.Resume1.UseVisualStyleBackColor = true;
            this.Resume1.Click += new System.EventHandler(this.Resume1_Click);
            // 
            // Thread1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 276);
            this.Controls.Add(this.Resume1);
            this.Controls.Add(this.Pause1);
            this.Name = "Thread1";
            this.Text = "Thread 1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Thread1_FormClosed);
            this.Load += new System.EventHandler(this.Thread1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Pause1;
        private System.Windows.Forms.Button Resume1;
    }
}