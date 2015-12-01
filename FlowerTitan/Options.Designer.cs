namespace FlowerTitan
{
    partial class Options
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxXLSPath = new System.Windows.Forms.TextBox();
            this.buttonSetPath = new System.Windows.Forms.Button();
            this.folderBrowserDialogXLSPath = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default XLS path:";
            // 
            // textBoxXLSPath
            // 
            this.textBoxXLSPath.Enabled = false;
            this.textBoxXLSPath.Location = new System.Drawing.Point(109, 26);
            this.textBoxXLSPath.Name = "textBoxXLSPath";
            this.textBoxXLSPath.Size = new System.Drawing.Size(382, 20);
            this.textBoxXLSPath.TabIndex = 1;
            // 
            // buttonSetPath
            // 
            this.buttonSetPath.Location = new System.Drawing.Point(497, 24);
            this.buttonSetPath.Name = "buttonSetPath";
            this.buttonSetPath.Size = new System.Drawing.Size(75, 23);
            this.buttonSetPath.TabIndex = 2;
            this.buttonSetPath.Text = "Set path";
            this.buttonSetPath.UseVisualStyleBackColor = true;
            this.buttonSetPath.Click += new System.EventHandler(this.buttonSetPath_Click);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 59);
            this.Controls.Add(this.buttonSetPath);
            this.Controls.Add(this.textBoxXLSPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxXLSPath;
        private System.Windows.Forms.Button buttonSetPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogXLSPath;
    }
}