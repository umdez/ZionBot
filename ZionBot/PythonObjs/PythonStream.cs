using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OtClientBot.PythonObjs
{
    internal class PythonStream : MemoryStream
    {
        RichTextBox _output;
        public PythonStream(RichTextBox textbox)
        {
            _output = textbox;
        }
        public override void Write(byte[] buffer, int offset,
        int count)
        {
            string text = Encoding.UTF8.GetString(buffer, offset,
            count);
            _output.AppendText(text);
            _output.ScrollToCaret();
        }
    }
}
