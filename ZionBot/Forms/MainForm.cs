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

namespace OtClientBot
{
    public partial class MainForm :  MetroFramework.Forms.MetroForm
    {

        public static MainForm instance;

        public MainForm()
        {
            instance = this;
            InitializeComponent();

        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            this.wpfPythonConsole.Child = new Shell();


            
            InitializeWalkerView();
            LoadTargetingView();
            LoadAutoLootView();

            LoadScriptingView();


        }



        public void WriteLogs()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(WriteLogs));
            }
            else
            {
                while (Utilities.LogWritter.Logs.Count > 0)
                {
                    rtxtLog.AppendText(Utilities.LogWritter.Logs.Dequeue());
                }
                rtxtLog.ScrollToCaret();
            }
        }

        private void lstSavedWaypoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtWaypointName.Text = lstSavedWaypoints.SelectedItem.ToString();
        }

        private void btnLogsClear_Click(object sender, EventArgs e)
        {
            rtxtLog.Clear();
        }

        private void tabController_TabIndexChanged(object sender, EventArgs e)
        {
        }


        // Auto Loot Tab

        private void toggleAutoLoot_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleAutoLoot.Checked)
            {

                _Cavebot.AutoLoot = new AutoLoot();
                _Cavebot.AutoLoot.Start();

            }
            else
            {
                if (_Cavebot.AutoLoot != null)
                {
                    _Cavebot.AutoLoot.Stop();
                }


            }
        }


    }
}
