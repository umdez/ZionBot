using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {


        static Breakpoint brIncomingPacket;
        static Breakpoint.BreakPointHandler OnRecievePacket;
        

        static void LoadIncomingPacketBreakPoint(bool StartOnRun)
        {

            OnRecievePacket = IncomingPacketCallBack;
            brIncomingPacket = new Breakpoint((uint)Address.Hooks.IncomingPacket, OnRecievePacket, StartOnRun);
        }


        static byte[] IncomingOpCodeBlackList = new byte[] { 0x1D, 0x1E, 0x6D, 0x8C };


        static void IncomingPacketCallBack(DEBUG_EVENT evt)
        {

            GetCtx();


            uint PacketAddres = ctx.Ecx;
            byte OpCode = Memory.ReadByte(ctx.Ecx + 0x1A);

            ushort PacketSize = Memory.ReadByte(PacketAddres + 0x18);// Using byte here, but full packet size is a word ( 2 bytes )

            byte[] rawPacket = Memory.ReadBytes(PacketAddres + 0x1A, (uint)PacketSize); 



            #region Setter

            byte[] opStatusBarMessage = { 0xB4, 0x17 };

            byte[] opMessage = { 0xAA };

            
            if ( rawPacket.Length > 5 &&  rawPacket[0] == opStatusBarMessage[0] && rawPacket[1] == opStatusBarMessage[1] )
            {
                 Client.LastStatusBarMessage = ASCIIEncoding.ASCII.GetString(rawPacket.Skip(4).Take(rawPacket.Length -4).ToArray());
            }




            #endregion


            #region Replacer



            // If packet contains something that is to be replaced then replace it.


            if (rawPacket.Length == 1 && rawPacket[0] == 0xA3 && false){ //(byte)Enums.IncomingOpCodes.GameServerClearTarget){

                if(Cavebot._Cavebot.isTargetingRunning )
                {
                    Program.Log("Debugger:IncomingPacketHook: Replaced ClearTarget with 0x00");
                    Memory.WriteByte(PacketAddres + 0x1A, 0x0);
                }
                
            }



            #endregion



            #region Logger



            if (IncomingOpCodeBlackList.Contains(OpCode) == false && ctx.Ecx != 0) //  Check if it is not a Blacklisted Packet
            {
                // Log incoming packet.
                string RecievedPacket = BitConverter.ToString(rawPacket).Replace("-", " ");

                Utilities.LogWritter.RecievedPackets.Enqueue(RecievedPacket);

            } 


            #endregion

            ContinueBreakPoint(brIncomingPacket, true);// Resume Execution
        }



















    }
}
