using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace OtClientBot.WpfControls
{
    /// <summary>
    /// Interaction logic for ScriptEditor.xaml
    /// </summary>
    public partial class ScriptEditor : UserControl
    {
        public ScriptEditor(string escripiti)
        {
            InitializeComponent();


            IHighlightingDefinition pythonHighlighting;
            using (Stream s = new MemoryStream(Encoding.UTF8.GetBytes(Properties.Resources.Python)))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    pythonHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            // and register it in the HighlightingManager
            HighlightingManager.Instance.RegisterHighlighting("Python Highlighting", new string[] { ".cool" }, pythonHighlighting);


            scriptEditor.SyntaxHighlighting = pythonHighlighting;


            this.scriptEditor.Text = escripiti;

        }
    }
}
