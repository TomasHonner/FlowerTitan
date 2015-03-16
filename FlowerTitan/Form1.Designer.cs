namespace FlowerTitan
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.iB4 = new Emgu.CV.UI.ImageBox();
            this.iB3 = new Emgu.CV.UI.ImageBox();
            this.iB2 = new Emgu.CV.UI.ImageBox();
            this.iB1 = new Emgu.CV.UI.ImageBox();
            this.tID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkboxFilter3 = new System.Windows.Forms.CheckBox();
            this.checkboxFilter2 = new System.Windows.Forms.CheckBox();
            this.checkboxFilter1 = new System.Windows.Forms.CheckBox();
            this.colorPickerBlue = new System.Windows.Forms.RadioButton();
            this.colorPickerGreen = new System.Windows.Forms.RadioButton();
            this.colorPickerRed = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iB4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iB3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iB2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iB1)).BeginInit();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.iB4);
            this.panel1.Controls.Add(this.iB3);
            this.panel1.Controls.Add(this.iB2);
            this.panel1.Controls.Add(this.iB1);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(599, 503);
            this.panel1.TabIndex = 0;
            // 
            // iB4
            // 
            this.iB4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iB4.Location = new System.Drawing.Point(300, 237);
            this.iB4.Name = "iB4";
            this.iB4.Size = new System.Drawing.Size(250, 217);
            this.iB4.TabIndex = 6;
            this.iB4.TabStop = false;
            // 
            // iB3
            // 
            this.iB3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iB3.Location = new System.Drawing.Point(3, 237);
            this.iB3.Name = "iB3";
            this.iB3.Size = new System.Drawing.Size(250, 217);
            this.iB3.TabIndex = 5;
            this.iB3.TabStop = false;
            // 
            // iB2
            // 
            this.iB2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iB2.Location = new System.Drawing.Point(300, 3);
            this.iB2.Name = "iB2";
            this.iB2.Size = new System.Drawing.Size(250, 217);
            this.iB2.TabIndex = 3;
            this.iB2.TabStop = false;
            // 
            // iB1
            // 
            this.iB1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iB1.Location = new System.Drawing.Point(3, 3);
            this.iB1.Name = "iB1";
            this.iB1.Size = new System.Drawing.Size(250, 217);
            this.iB1.TabIndex = 2;
            this.iB1.TabStop = false;
            // 
            // tID
            // 
            this.tID.Location = new System.Drawing.Point(805, 27);
            this.tID.Name = "tID";
            this.tID.Size = new System.Drawing.Size(100, 20);
            this.tID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(733, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Template ID";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkboxFilter3);
            this.panel2.Controls.Add(this.checkboxFilter2);
            this.panel2.Controls.Add(this.checkboxFilter1);
            this.panel2.Controls.Add(this.colorPickerBlue);
            this.panel2.Controls.Add(this.colorPickerGreen);
            this.panel2.Controls.Add(this.colorPickerRed);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(674, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(231, 159);
            this.panel2.TabIndex = 3;
            // 
            // checkboxFilter3
            // 
            this.checkboxFilter3.AutoSize = true;
            this.checkboxFilter3.Location = new System.Drawing.Point(17, 92);
            this.checkboxFilter3.Name = "checkboxFilter3";
            this.checkboxFilter3.Size = new System.Drawing.Size(54, 17);
            this.checkboxFilter3.TabIndex = 7;
            this.checkboxFilter3.Text = "Filter3";
            this.checkboxFilter3.UseVisualStyleBackColor = true;
            // 
            // checkboxFilter2
            // 
            this.checkboxFilter2.AutoSize = true;
            this.checkboxFilter2.Location = new System.Drawing.Point(17, 69);
            this.checkboxFilter2.Name = "checkboxFilter2";
            this.checkboxFilter2.Size = new System.Drawing.Size(54, 17);
            this.checkboxFilter2.TabIndex = 6;
            this.checkboxFilter2.Text = "Filter2";
            this.checkboxFilter2.UseVisualStyleBackColor = true;
            // 
            // checkboxFilter1
            // 
            this.checkboxFilter1.AutoSize = true;
            this.checkboxFilter1.Location = new System.Drawing.Point(17, 46);
            this.checkboxFilter1.Name = "checkboxFilter1";
            this.checkboxFilter1.Size = new System.Drawing.Size(54, 17);
            this.checkboxFilter1.TabIndex = 5;
            this.checkboxFilter1.Text = "Filter1";
            this.checkboxFilter1.UseVisualStyleBackColor = true;
            // 
            // colorPickerBlue
            // 
            this.colorPickerBlue.AutoSize = true;
            this.colorPickerBlue.Location = new System.Drawing.Point(112, 92);
            this.colorPickerBlue.Name = "colorPickerBlue";
            this.colorPickerBlue.Size = new System.Drawing.Size(46, 17);
            this.colorPickerBlue.TabIndex = 4;
            this.colorPickerBlue.TabStop = true;
            this.colorPickerBlue.Text = "Blue";
            this.colorPickerBlue.UseVisualStyleBackColor = true;
            // 
            // colorPickerGreen
            // 
            this.colorPickerGreen.AutoSize = true;
            this.colorPickerGreen.Location = new System.Drawing.Point(112, 69);
            this.colorPickerGreen.Name = "colorPickerGreen";
            this.colorPickerGreen.Size = new System.Drawing.Size(54, 17);
            this.colorPickerGreen.TabIndex = 3;
            this.colorPickerGreen.TabStop = true;
            this.colorPickerGreen.Text = "Green";
            this.colorPickerGreen.UseVisualStyleBackColor = true;
            // 
            // colorPickerRed
            // 
            this.colorPickerRed.AutoSize = true;
            this.colorPickerRed.Location = new System.Drawing.Point(112, 46);
            this.colorPickerRed.Name = "colorPickerRed";
            this.colorPickerRed.Size = new System.Drawing.Size(45, 17);
            this.colorPickerRed.TabIndex = 2;
            this.colorPickerRed.TabStop = true;
            this.colorPickerRed.Text = "Red";
            this.colorPickerRed.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(109, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Line colors";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(14, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Filters";
            // 
            // buttonStop
            // 
            this.buttonStop.ForeColor = System.Drawing.Color.Red;
            this.buttonStop.Location = new System.Drawing.Point(674, 395);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(231, 40);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop Processing";
            this.buttonStop.UseVisualStyleBackColor = true;
            // 
            // buttonStart
            // 
            this.buttonStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonStart.Location = new System.Drawing.Point(674, 350);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(231, 40);
            this.buttonStart.TabIndex = 5;
            this.buttonStart.Text = "Start Processing";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(674, 471);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(231, 40);
            this.buttonExport.TabIndex = 6;
            this.buttonExport.Text = "Export to XLS";
            this.buttonExport.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(918, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTemplateToolStripMenuItem,
            this.saveTemplateToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadTemplateToolStripMenuItem
            // 
            this.loadTemplateToolStripMenuItem.Name = "loadTemplateToolStripMenuItem";
            this.loadTemplateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadTemplateToolStripMenuItem.Text = "Load template";
            this.loadTemplateToolStripMenuItem.Click += new System.EventHandler(this.loadTemplateToolStripMenuItem_Click);
            // 
            // saveTemplateToolStripMenuItem
            // 
            this.saveTemplateToolStripMenuItem.Name = "saveTemplateToolStripMenuItem";
            this.saveTemplateToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveTemplateToolStripMenuItem.Text = "Save template";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 540);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tID);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iB4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iB3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iB2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iB1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Emgu.CV.UI.ImageBox iB2;
        private Emgu.CV.UI.ImageBox iB1;
        private Emgu.CV.UI.ImageBox iB4;
        private Emgu.CV.UI.ImageBox iB3;
        private System.Windows.Forms.TextBox tID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton colorPickerBlue;
        private System.Windows.Forms.RadioButton colorPickerGreen;
        private System.Windows.Forms.RadioButton colorPickerRed;
        private System.Windows.Forms.CheckBox checkboxFilter3;
        private System.Windows.Forms.CheckBox checkboxFilter2;
        private System.Windows.Forms.CheckBox checkboxFilter1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTemplateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}

