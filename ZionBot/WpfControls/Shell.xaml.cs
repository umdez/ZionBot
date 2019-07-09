using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace OtClientBot
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : UserControl
    {
        public Shell()
        {       
            InitializeComponent();
        }


        private async void ItLoaded(object sender, RoutedEventArgs e)
        {

            // This will ensure the console is ready before setting variables.
            await Task.Factory.StartNew(() => {
                while (true)
                {
                    try
                    {
                        if (wpfPythonConsole != null &&
                            wpfPythonConsole.Pad != null &&
                            wpfPythonConsole.Pad.Console != null &&
                            wpfPythonConsole.Pad.Console.ScriptScope != null)
                        {
                            wpfPythonConsole.Pad.Console.ScriptScope.SetVariable("ready", true);
                            break;
                        }
                    }
                    catch { }


                    Thread.Sleep(500);
                }
                });

            //wpfPythonConsole.Pad.Control.WordWrap = true;

            //wpfPythonConsole.Pad.Host.ConsoleCreated += (evt, s) =>
            //{
            //    wpfPythonConsole.Pad.Console.ConsoleInitialized += (_evt, _s) =>
            //    {

            //    };
            //};



            ScriptingEngine.PopulateScope(wpfPythonConsole.Pad.Console.ScriptScope);

        }
    }
}
