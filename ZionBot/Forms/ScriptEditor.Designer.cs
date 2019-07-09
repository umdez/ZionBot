namespace OtClientBot.Forms
{
    partial class ScriptEditor
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
            this.wpfScriptEditor = new System.Windows.Forms.Integration.ElementHost();
            this.btnSave = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // wpfScriptEditor
            // 
            this.wpfScriptEditor.Location = new System.Drawing.Point(23, 63);
            this.wpfScriptEditor.Name = "wpfScriptEditor";
            this.wpfScriptEditor.Size = new System.Drawing.Size(602, 314);
            this.wpfScriptEditor.TabIndex = 0;
            this.wpfScriptEditor.Child = null;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(504, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 24);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 400);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.wpfScriptEditor);
            this.MaximizeBox = false;
            this.Name = "ScriptEditor";
            this.Resizable = false;
            this.Text = "ScriptEditor";
            this.Theme = MetroFramework.MetroThemeStyle.Light;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost wpfScriptEditor;
        private MetroFramework.Controls.MetroButton btnSave;
    }
}