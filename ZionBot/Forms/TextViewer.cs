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
    public partial class TextViewer : MetroForm
    {
        public delegate string getText();

        public delegate void _setText(string text);


        private void Initialize(string title = null)
        {
            this.rtxtText.Location = new Point(23, 63);

            this.rtxtText.Size = new Size(this.Width - 23 * 2, this.Height - 63 - 25);

            if (title != null)
            {
                this.Text = title;

            }
        }

        public TextViewer(getText getTextDelegate, string title= null)
        {
            InitializeComponent();   

            Initialize(title);


            // Por enquanto iremos usar essa abordagem de atualizar a cada 1 segundo. 
            // Porém o ideal seria ter um Event Handler


            _setText updateTextAction = new _setText(setText);

            System.Threading.Tasks.Task.Factory.StartNew(() => {

                while (this.IsDisposed == false)
                {
                    var str = getTextDelegate();


                    this.rtxtText.Invoke(updateTextAction,new object[] { str });

                    

                    System.Threading.Thread.Sleep(1000);
                }
            });


        }


        public void setText(string text)
        {
            if (chkAutoRefresh.Checked)
            {
                this.rtxtText.Text = text;
                this.rtxtText.Select(text.Length - 1, 0);
                this.rtxtText.ScrollToCaret();
            }
        }

        public TextViewer(string text,string title = null)
        {
            InitializeComponent();
            

            Initialize(title);

            this.rtxtText.Text = text;


        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            this.rtxtText.Size = new Size(this.Width - 23 * 2, this.Height - 63 - 25);

        }

        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = chkTopMost.Checked;
        }
    }
}
