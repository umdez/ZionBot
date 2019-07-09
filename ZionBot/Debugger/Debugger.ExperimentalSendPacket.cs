using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {


        static bool doSendPacket = false;



        
        public static void _DoSendPacket(Packet p)
        {
            doSendPacket = true;
            PacketToSend = p;
            brMainLoop.Activated = true;
        }






    }
}



