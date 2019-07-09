namespace OtClientBot.Forms
{
    partial class TextViewer
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
            this.rtxtText = new System.Windows.Forms.RichTextBox();
            this.chkAutoRefresh = new MetroFramework.Controls.MetroToggle();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.chkTopMost = new MetroFramework.Controls.MetroToggle();
            this.SuspendLayout();
            // 
            // rtxtText
            // 
            this.rtxtText.Location = new System.Drawing.Point(23, 63);
            this.rtxtText.Name = "rtxtText";
            this.rtxtText.Size = new System.Drawing.Size(371, 132);
            this.rtxtText.TabIndex = 0;
            this.rtxtText.Text = "";
            // 
            // chkAutoRefresh
            // 
            this.chkAutoRefresh.AutoSize = true;
            this.chkAutoRefresh.Checked = true;
            this.chkAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoRefresh.Location = new System.Drawing.Point(117, 9);
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.Size = new System.Drawing.Size(80, 17);
            this.chkAutoRefresh.TabIndex = 1;
            this.chkAutoRefresh.Text = "On";
            this.chkAutoRefresh.UseVisualStyleBackColor = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(27, 7);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(84, 19);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Auto Refresh";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(203, 7);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(65, 19);
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Text = "Top Most";
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(274, 9);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(80, 17);
            this.chkTopMost.TabIndex = 3;
            this.chkTopMost.Text = "Off";
            this.chkTopMost.UseVisualStyleBackColor = true;
            this.chkTopMost.CheckedChanged += new System.EventHandler(this.chkTopMost_CheckedChanged);
            // 
            // TextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 218);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.chkTopMost);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.chkAutoRefresh);
            this.Controls.Add(this.rtxtText);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "TextViewer";
            this.Text = "Text Viewer";
            this.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TopMost = true;
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtText;
        private MetroFramework.Controls.MetroToggle chkAutoRefresh;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroToggle chkTopMost;
    }
}