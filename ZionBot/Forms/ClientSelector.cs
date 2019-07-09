using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OtClientBot
{
    public partial class ClientSelector : MetroFramework.Forms.MetroForm
    {

        Dictionary<string, Process> Clients = new Dictionary<string, Process>();



        public ClientSelector()
        {
            InitializeComponent();
        }

        private void ClientSelector_Load(object sender, EventArgs e)
        {
            LoadClients();
        }


        private void LoadClients()
        {
            listClientList.Items.Clear();
            Clients.Clear();
                                              
            Process[] processes = Process.GetProcessesByName("Medivia_D3D");

            if (processes.Length == 0) return;

            foreach(Process p in processes)
            {

                string charName = GetCharacterName(p);

                if (charName.Length < 2) charName = "<Not Logged in>";

                string PID = p.Id.ToString();

                string signature = charName + " [" + PID + "]";

                Clients.Add(signature,p);
                listClientList.Items.Add(signature);
            }

            if (listClientList.Items.Count > 0) listClientList.SelectedIndex = 0;

        }




        private string GetCharacterName(Process p)
        {
            IntPtr BaseAddress = p.MainModule.BaseAddress;
            Memory.TargetHandle = p.Handle;

            IntPtr PlayerPointer = BaseAddress + Address.adrPlayer.PlayerPointer;
            uint PlayerStart = Memory.ReadUint((long)PlayerPointer);
            string Name = Memory.ReadSdtString(PlayerStart + 0x20);
            return Name;

        }


    private void SelectSelected()
        {
            if (listClientList.SelectedIndex < 0) return;


            Client.process = Clients[(string)listClientList.SelectedItem];


            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void listClientList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void onDoubleClick(object sender, EventArgs e)
        {
            SelectSelected();
        }

        private void btnMetroSelect(object sender, EventArgs e)
        {
            SelectSelected();
        }

        private void btnMetroRefresh(object sender, EventArgs e)
        {
            LoadClients();
        }
    }
    }
