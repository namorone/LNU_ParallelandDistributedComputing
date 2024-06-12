namespace Forms
{
    partial class Thread4
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
            this.Resume4 = new System.Windows.Forms.Button();
            this.Pause4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Resume4
            // 
            this.Resume4.Location = new System.Drawing.Point(158, 237);
            this.Resume4.Name = "Resume4";
            this.Resume4.Size = new System.Drawing.Size(93, 33);
            this.Resume4.TabIndex = 1;
            this.Resume4.Text = "Resume";
            this.Resume4.UseVisualStyleBackColor = true;
            this.Resume4.Click += new System.EventHandler(this.Resume4_Click);
            // 
            // Pause4
            // 
            this.Pause4.Location = new System.Drawing.Point(34, 237);
            this.Pause4.Name = "Pause4";
            this.Pause4.Size = new System.Drawing.Size(93, 33);
            this.Pause4.TabIndex = 3;
            this.Pause4.Text = "Pause";
            this.Pause4.UseVisualStyleBackColor = true;
            this.Pause4.Click += new System.EventHandler(this.Pause4_Click);
            // 
            // Thread4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 276);
            this.Controls.Add(this.Resume4);
            this.Controls.Add(this.Pause4);
            this.Name = "Thread4";
            this.Text = "Thread 4";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Thread4_FormClosed);
            this.Load += new System.EventHandler(this.Thread4_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Resume4;
        private System.Windows.Forms.Button Pause4;
    }
}