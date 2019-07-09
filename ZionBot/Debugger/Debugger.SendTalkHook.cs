using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using static OtClientBot.WinApi;
namespace OtClientBot
{
    public partial class Debugger
    {


        static Breakpoint brSendTalk;
        static Breakpoint.BreakPointHandler OnSendTalk;
        
    
        static void LoadSendTalkBreakPoint(bool StartOnRun)
        {
            OnSendTalk = SendTalkCallBack;
            brSendTalk = new Breakpoint((uint)Address.Hooks.SendTalk, OnSendTalk, StartOnRun);
        }


        static void SendTalkCallBack(DEBUG_EVENT evt)
        {
            GetCtx();  
            uint strAddress = Memory.ReadUint(ctx.Esp + 0x10);
            string TextMessage = Memory.ReadSdtString(strAddress);

            Program.Log("Send Talk:" + TextMessage);            

            if (TextMessage.Length > 2 && TextMessage[0] == Preferences.CommandPrefix)
            {
                Program.Log("It was a command!");
                ProcessCommand(TextMessage);
                Memory.WriteSdrString(strAddress, "");

            }


            ContinueBreakPoint(brSendTalk, true);
        }


        static ushort itemId=0;
        private static void ProcessCommand(string textMessage)
        {
            if (textMessage.Contains("  ")) // If it contains double space
            {
                textMessage = Regex.Replace(textMessage, @"\s+", " ");
            }
            string[] Parts = textMessage.Split(new char[] { ' ' },2);

            Parts[0] = Parts[0].TrimStart(Preferences.CommandPrefix);

            switch (Parts[0].ToLower())
            {
                #region AimBot
                case ("useontarget"):
                case ("useoncreature"):
                case ("usetarget"):
                case ("usecreature"):
                case ("target"):
                    // This is very raw because we want it to be fast.
                    uint creatureAddress = Player.AttackingCreaturePtr;
                    if (creatureAddress != 0)
                    {
                        if (ushort.TryParse(Parts[1], out itemId) == false)
                        {
                            Client.Output("Unknown item Id.");
                            break;
                        }
                        BitConverter.GetBytes(itemId).CopyTo(useWithPacket, 6); // Copy item id to packet.

                        BitConverter.GetBytes((short)Memory.ReadUShort(creatureAddress + 0xC)).CopyTo(useWithPacket, 9); // Copy X
                        BitConverter.GetBytes((short)Memory.ReadUShort(creatureAddress + 0x10)).CopyTo(useWithPacket, 11); // Y
                        useWithPacket[13] = (byte)Memory.ReadByte(creatureAddress + 0x14);// Z

                        new Packet(useWithPacket).SendPacket(); // Send the packet.
                    }
                    else
                    {
                        Client.Output("No target found.");
                    }
                    break;
                case ("useonself"):
                case ("useself"):
                case ("self"):
                    if (ushort.TryParse(Parts[1], out itemId) == false)
                    {
                        Client.Output("Unknown item Id.");
                        break;
                    }

                    BitConverter.GetBytes(itemId).CopyTo(useWithPacket, 6); // Copy item id to packet.

                    BitConverter.GetBytes(Player.X).CopyTo(useWithPacket, 9); // Copy X
                    BitConverter.GetBytes(Player.Y).CopyTo(useWithPacket, 11); // Y
                    useWithPacket[13] = Player.Z; // Z

                    new Packet(useWithPacket).SendPacket(); // SendPacket
                    break; 
                #endregion
                case ("sendpacket"):
                case ("send"):
                    if (Parts.Count() > 1)
                        SendPacket(new Packet(Utils.StringToByteArrayFastest(Parts[1])));
                    break;

                case ("battlelist"):
                case ("printcreatures"):
                case ("printbattlelist"):
                    foreach(Creature c in BattleList.GetAllCreatures(true))
                    {
                        c.Print();
                    }
                    break;
                case ("iventory"):
                case ("printcontainers"):
                case ("printiventory"):
                    Iventory.PrintAllEquips();
                    Iventory.PrintAllContainers();
                    break;
                case ("fulllight"):
                    Client.SetLight(255);
                    break;
                case ("light"):
                    byte itensity = 0;

                    if (byte.TryParse(Parts[1], out itensity) == false)
                    {
                        Client.Output("Could not parse light itensity byte.");
                        break;
                    }
                    Client.SetLight(itensity);
                    break;
                case ("getslot"):
                    byte slotnum = 0;

                    if (byte.TryParse(Parts[1], out slotnum) == false)
                    {
                        Client.Output("Could not parse slot byte.");
                        break;
                    }
                    Iventory.GetSlot((Iventory.EquipSlot)(slotnum)).Print();

                    break;

                case ("fish"):
                    Modules.AutoFishing.Start();
                    break;

                case ("logincomingpackets"):
                    brIncomingPacket.Activated = !brIncomingPacket.Activated;
                    break;
                case ("logsendingpackets"):
                    brSendingPacket.Activated = !brSendingPacket.Activated;
                    break;
                case ("clear"):
                    System.Threading.Tasks.Task.Factory.StartNew(() => Draw.ClearBoard());
                    break;
                case ("attacknear"):
                    System.Threading.Tasks.Task.Factory.StartNew(() => Player.AttackCreature(BattleList.GetNearCreature(1)));                    
                    break;
                case ("attack"):
                    System.Threading.Tasks.Task.Factory.StartNew(() => Player.AttackCreature(BattleList.GetNearCreature(7)));
                    break;
                case ("attackall"):
                    System.Threading.Tasks.Task.Factory.StartNew(() => { foreach (Creature c in BattleList.GetAllCreatures(true)) { Player.AttackCreature(c); Thread.Sleep(500); } } );
                    break;    


                case ("clickself"):
                    System.Threading.Tasks.Task.Factory.StartNew(() => Post.leftClick(Find.Self()));
                    break;
                
                default:
                    Program.Log("Unknown Command: " + Preferences.CommandPrefix +Parts[0]);
                    break;
                   





            }






        }

        private static void Bitmap(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
