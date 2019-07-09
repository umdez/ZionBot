using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtClientBot.Forms
{
    public partial class ScriptEditor : MetroForm
    {
        private string path;

        public ScriptEditor(string text, string path,string name)
        {
            InitializeComponent();
            this.path = path;
            this.Text = "Editing script: " + name;
            this.wpfScriptEditor.Child = new WpfControls.ScriptEditor(text);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(this.path,((WpfControls.ScriptEditor)this.wpfScriptEditor.Child).scriptEditor.Text);
            this.Close();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (isCtrl && e.KeyChar.ToString().ToLower() == "s" )
            {
                System.IO.File.WriteAllText(this.path, ((WpfControls.ScriptEditor)this.wpfScriptEditor.Child).scriptEditor.Text);
                this.Close();
            }
        }

        private bool isCtrl = false;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey)
            {
                isCtrl = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey)
            {
                isCtrl = false;
            }
        }
    }
}
