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

        WpfControls.TargetingPolicyPanel TargetingPolicyPanel;
        TargetPolicy defaultPolicy;

        //ItemCollection TargetingPolicies;

        ObservableCollection<TargetPolicy> TargetingPolicies;




        public void LoadTargetingView()
        {
            this.wpfTargetingPolicyPanel.Child = new WpfControls.TargetingPolicyPanel();
            TargetingPolicyPanel = (WpfControls.TargetingPolicyPanel)this.wpfTargetingPolicyPanel.Child;


            this.defaultPolicy = new TargetPolicy("Default", Enums.Priority.Medium, false, Enums.TargetingMode.Follow, isDefault: true);
            TargetingPolicies = new ObservableCollection<TargetPolicy>();
            TargetingPolicies.Add(defaultPolicy);


            TargetingPolicyPanel.ViewModel.PolicyListSource = TargetingPolicies;
            




        }



        private void btnTargetingAddNewPolicy_Click(object sender, EventArgs e)
        {
            
            TargetingPolicies.Add(new TargetPolicy("",this.defaultPolicy.priority,!this.defaultPolicy.ignore,this.defaultPolicy.targetingMode));
        }

        private void btnTargetingClearPolicyList_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the policy list?", "Press Yes to confirm.", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                TargetingPolicies.Clear();
                TargetingPolicies.Add(defaultPolicy);

            }
        }

        private void btnTargetingSavePolicies_Click(object sender, EventArgs e)
        {

        }

        private void btnTargetingLoadPolicies_Click(object sender, EventArgs e)
        {

        }



        private void toggleTargeting_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleTargeting.Checked)
            {

                // Deactivate buttons:
                btnTargetingAddNewPolicy.Enabled = false;
                btnTargetingClearPolicyList.Enabled = false;
                btnTargetingLoadPolicies.Enabled = false;
                btnTargetingSavePolicies.Enabled = false;

                chkAttackAllCreaturesThatAttack.Enabled = false;
                chkSwitchToHigherPriority.Enabled = false;





                // Create the parameter objects:

                // Seta o TargetPolicy como sendo o mesmo que aparece na View... Logo, adicionar faz adicionar, remover remove, etc..
                ObservableCollection<TargetPolicy> tPolicies = TargetingPolicies;

                tPolicies.Remove(defaultPolicy);

                bool switchToHigherPriority = chkSwitchToHigherPriority.Checked;


                // Make Targeting Object

                _Cavebot.Targeting = new Targeting(tPolicies, defaultPolicy, switchToHigherPriority);




                // Start it

                _Cavebot.Targeting.Start();

            }
            else
            {

                // Stops the Targeting.


                if (_Cavebot.Targeting != null && _Cavebot.Targeting.isAlive()) _Cavebot.Targeting.Stop();



                // Removes the Targeting Object


                _Cavebot.Targeting = null;




                // Activate buttons:


                btnTargetingAddNewPolicy.Enabled = true;
                btnTargetingClearPolicyList.Enabled = true;
                btnTargetingLoadPolicies.Enabled = true;
                btnTargetingSavePolicies.Enabled = true;

                chkAttackAllCreaturesThatAttack.Enabled = true;
                chkSwitchToHigherPriority.Enabled = true;



            }


        }


    }
}
