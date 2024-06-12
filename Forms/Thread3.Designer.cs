namespace Forms
{
    partial class Thread3
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
            this.Resume3 = new System.Windows.Forms.Button();
            this.Pause3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Resume3
            // 
            this.Resume3.Location = new System.Drawing.Point(158, 237);
            this.Resume3.Name = "Resume3";
            this.Resume3.Size = new System.Drawing.Size(93, 33);
            this.Resume3.TabIndex = 1;
            this.Resume3.Text = "Resume";
            this.Resume3.UseVisualStyleBackColor = true;
            this.Resume3.Click += new System.EventHandler(this.Resume3_Click);
            // 
            // Pause3
            // 
            this.Pause3.Location = new System.Drawing.Point(34, 237);
            this.Pause3.Name = "Pause3";
            this.Pause3.Size = new System.Drawing.Size(93, 33);
            this.Pause3.TabIndex = 3;
            this.Pause3.Text = "Pause";
            this.Pause3.UseVisualStyleBackColor = true;
            this.Pause3.Click += new System.EventHandler(this.Pause3_Click);
            // 
            // Thread3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 276);
            this.Controls.Add(this.Resume3);
            this.Controls.Add(this.Pause3);
            this.Name = "Thread3";
            this.Text = "Thread 3";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Thread3_FormClosed);
            this.Load += new System.EventHandler(this.Thread3_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Resume3;
        private System.Windows.Forms.Button Pause3;
    }
}