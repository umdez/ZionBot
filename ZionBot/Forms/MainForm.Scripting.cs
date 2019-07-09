using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime.Types;
using OtClientBot.Cavebot;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace OtClientBot
{
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {

        WpfControls.ScriptingMananger ScriptingManangerControl;

        





        public void LoadScriptingView()
        {
            this.wpfScriptsHost.Child = new WpfControls.ScriptingMananger();

            ScriptingManangerControl = (WpfControls.ScriptingMananger)this.wpfScriptsHost.Child;


            //ScriptingManangerControl.LoadScriptList();





            // Cria a lista de running Scripts
            //ScriptingEngine.runningScripts = new ObservableCollection<RunningScript>();

            //RunningScriptsControl.ViewModel.RunningScriptsList = ScriptingEngine.runningScripts;
            




        }



        private void btnNewScript_Click(object sender, EventArgs e)
        {
            ScriptingManangerControl.NewScript();
        }


    }
}
