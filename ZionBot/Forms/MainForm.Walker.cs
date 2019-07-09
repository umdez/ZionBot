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
    public partial class MainForm : MetroFramework.Forms.MetroForm
    {



        public void InitializeWalkerView()
        {
            LoadSavedWayPoints();

            this.rbtnC.Checked = true;


        }

        public void UpdateWalkerView()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateWalkerView));
            }
            else
            {
                lstWaypoints.SelectedIndex = Cavebot._Cavebot.Walker.CurrentWaypointIndex;
               
            }
        }





        private void toggleWalker_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleWalker.Checked)
            {
                if (lstWaypoints.Items.Count ==0)
                {
                    toggleWalker.Checked = false;
                    return;
                }


                _Cavebot.Walker = new Walker(lstWaypoints.Items.OfType<Waypoint>().ToList());

                if(lstWaypoints.SelectedIndex>0)_Cavebot.Walker.CurrentWaypointIndex = lstWaypoints.SelectedIndex;

                _Cavebot.Walker.Start();




            }
            else
            {
                if (_Cavebot.Walker != null)
                {
                    _Cavebot.Walker.Stop();
                }

                lstWaypoints.SelectedIndex = -1;

            }
        }

               
        

        private Location GetWalkerLocation()
        {
            var loc =  Player.Location;

            if (rbtnN.Checked) loc.addOffset(0, -1);
            else if (rbtnNE.Checked) loc.addOffset(1, -1);
            else if (rbtnNW.Checked) loc.addOffset(-1, -1);
            else if (rbtnE.Checked) loc.addOffset(1, 0);
            else if (rbtnW.Checked) loc.addOffset(-1, 0);
            else if (rbtnSE.Checked) loc.addOffset(1, 1);
            else if (rbtnSW.Checked) loc.addOffset(-1, 1);
            else if (rbtnS.Checked) loc.addOffset(0, 1);

            return loc;
        }


        private void btnWaypointUp_Click(object sender, EventArgs e)
        {
            var selectedIndex = lstWaypoints.SelectedIndex;
            if (selectedIndex <= 0) return;
            var item = lstWaypoints.SelectedItem;

            lstWaypoints.Items.RemoveAt(selectedIndex);
            lstWaypoints.Items.Insert(selectedIndex - 1, item);
            lstWaypoints.SelectedIndex = selectedIndex - 1;
        }

        private void btnWaypointDown_Click(object sender, EventArgs e)
        {
            var selectedIndex = lstWaypoints.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex+1 == lstWaypoints.Items.Count ) return;
            var item = lstWaypoints.SelectedItem;

            lstWaypoints.Items.RemoveAt(selectedIndex);
            lstWaypoints.Items.Insert(selectedIndex + 1, item);
            lstWaypoints.SelectedIndex = selectedIndex + 1;
        }

        private void btnWaypointEdit_Click(object sender, EventArgs e)
        {
            var text = Microsoft.VisualBasic.Interaction.InputBox("Edit your waypoint below.\nFormat: [XXXX,YYYY,Z,Arg]\nArg is the option your waypoint uses.\nWalk arg is node size, UseItem arg is item ID, \nScript arg is the Script. Etc.", "Waypoint Edit", lstWaypoints.SelectedItem.ToString());
            if(text[0]=='[' && text[-1] == ']')
            {
                if (text.Split(',').Length == 4)
                {
                    var parse = text.Replace("[", "").Replace("]", "").Split(',');
                    uint X, Y, Z;

                    if (UInt32.TryParse(parse[0], out X) && UInt32.TryParse(parse[1], out Y) && UInt32.TryParse(parse[2], out Z))
                    {
                        if(X<65355 && Y < 65355 & Z < 18)
                        {
                            uint argInt;
                            switch (((Waypoint)lstWaypoints.SelectedItem).Action)
                            {
                                case Enums.Action.Walk:
                                case Enums.Action.UseItemOn:
                                    if (!UInt32.TryParse(parse[3],out argInt))
                                    {
                                        MessageBox.Show("Invalid argument. It must be a number.");
                                        return;
                                    }
                                    break;   
                            }

                            var selectedIndex = lstWaypoints.SelectedIndex;

                            lstWaypoints.Items.RemoveAt(selectedIndex);
                            lstWaypoints.Items.Insert(selectedIndex, new Waypoint(text));
                            lstWaypoints.SelectedIndex = selectedIndex;
                            return;
                        }
                    }
                }
            }
            MessageBox.Show("Invalid format.");

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the waypoint list?","Press Yes to confirm.",MessageBoxButtons.YesNo);

            if(result == DialogResult.Yes)
            {
                lstWaypoints.Items.Clear();
            }


        }


        private void addWaypoint(Enums.Action action, string Arg = "")
        {
            lstWaypoints.Items.Add(new Waypoint(action, GetWalkerLocation(), Arg));


        }

        private void btnWalk_Click(object sender, EventArgs e)
        {
            addWaypoint(Enums.Action.Walk, "1");
        }

        private void btnUse_Click(object sender, EventArgs e)
        {
            addWaypoint(Enums.Action.Use);

        }

        private void btnStand_Click(object sender, EventArgs e)
        {
            addWaypoint(Enums.Action.Stand);

        }

        private void btnRope_Click(object sender, EventArgs e)
        {
            addWaypoint(Enums.Action.Rope);

        }

        private void btnShovel_Click(object sender, EventArgs e)
        {
            addWaypoint(Enums.Action.Shovel);
        }

        private void toggleTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = metroToggle1.Checked;
        }

        private void btnSaveWpt_Click(object sender, EventArgs e)
        {
            string waypointName = txtWaypointName.Text;


            var wptString = GetWayPointText();

            if (!string.IsNullOrWhiteSpace(wptString) && !string.IsNullOrWhiteSpace(waypointName))
            {
                System.IO.File.WriteAllText("waypoints/" + waypointName, wptString);

            }
            else
            {
                MessageBox.Show("Waypoint list or waypoint name are empty.");
            }
            LoadSavedWayPoints();

        }

        private void LoadSavedWayPoints()
        {
            lstSavedWaypoints.Items.Clear();
            if (System.IO.Directory.Exists("waypoints") == false)
            {
                System.IO.Directory.CreateDirectory("waypoints");
            }

            foreach(string waypointName in System.IO.Directory.EnumerateFiles("waypoints"))
            {
                lstSavedWaypoints.Items.Add(waypointName.Split('\\')[1]);
            }
        }



        private string GetWayPointText()
        {
            string res = "";
            foreach(Waypoint wpt in lstWaypoints.Items)
            {
                res += wpt.ToString().Split('#')[0].Replace(" ","");
                res += '\n';
            }
            return res;
        }

        private void btnLoadWpt_Click(object sender, EventArgs e)
        {
            lstWaypoints.Items.Clear();
            object selectedWpt = lstSavedWaypoints.SelectedItem;
            if (selectedWpt!=null && !string.IsNullOrWhiteSpace(selectedWpt.ToString()))
            {
                foreach (string wptString in System.IO.File.ReadAllLines("waypoints/" + selectedWpt.ToString()))
                {
                    lstWaypoints.Items.Add(new Waypoint(wptString));
                }
            }
        }

        private void btnDeleteWpt_Click(object sender, EventArgs e)
        {
            object selectedWpt = lstSavedWaypoints.SelectedItem;
            if (selectedWpt != null && string.IsNullOrWhiteSpace(selectedWpt.ToString()))
            {
                System.IO.File.Delete("waypoints/" + selectedWpt.ToString());
            }
            LoadSavedWayPoints();
        }

        private void btnWaypointDelete_Click(object sender, EventArgs e)
        {
            lstWaypoints.Items.Remove(lstWaypoints.SelectedItem);
        }


    }
}