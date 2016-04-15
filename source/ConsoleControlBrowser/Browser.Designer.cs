namespace ConsoleControlBrowser
{
    partial class Browser
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
            this.Tabs = new System.Windows.Forms.CustomTabControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Tabs
            // 
            this.Tabs.DisplayStyle = System.Windows.Forms.TabStyle.Chrome;
            // 
            // 
            // 
            this.Tabs.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.Control;
            this.Tabs.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ActiveCaption;
            this.Tabs.DisplayStyleProvider.BorderColorSelected = System.Drawing.SystemColors.Control;
            this.Tabs.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.Tabs.DisplayStyleProvider.CloserColorActive = System.Drawing.Color.White;
            this.Tabs.DisplayStyleProvider.FocusTrack = false;
            this.Tabs.DisplayStyleProvider.HotTrack = true;
            this.Tabs.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Tabs.DisplayStyleProvider.Opacity = 1F;
            this.Tabs.DisplayStyleProvider.Overlap = 16;
            this.Tabs.DisplayStyleProvider.Padding = new System.Drawing.Point(15, 5);
            this.Tabs.DisplayStyleProvider.Radius = 16;
            this.Tabs.DisplayStyleProvider.ShowTabCloser = true;
            this.Tabs.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
            this.Tabs.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.Tabs.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Tabs.HotTrack = true;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(737, 609);
            this.Tabs.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(702, 5);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.button1.Size = new System.Drawing.Size(25, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Browser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(737, 609);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Tabs);
            this.Name = "Browser";
            this.Text = "Commandeer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CustomTabControl Tabs;
        private System.Windows.Forms.Button button1;
    }
}

