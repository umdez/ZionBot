using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {
        static Queue<Packet> SendingPackets = new Queue<Packet>();
        


        public static void SendPacket(Packet p)
        {
            SendingPackets.Enqueue(p);
            Commands.Enqueue(Command.SendPacket);
        }

        private static Packet PacketToSend;

        private static void ExecuteSendPacket(DEBUG_EVENT evt)
        {
            if (SendingPackets.Count > 0)
            {
                PacketToSend = SendingPackets.Dequeue(); // Get Packet


                // Modify the context so the instruction "call esi" will actually call our function

                PacketToSend.WritePacket(Client.PacketAddress);


                Memory.WriteUint(_MainCtx.ctx.Esp + 4, Client.PacketAddress); // Mov [ESP + 4] , PacketAddress
                Memory.WriteUint(_MainCtx.ctx.Esp, _MainCtx.ctx.Esp + 4);          // Move [ESP] , ESP + 4


                _MainCtx.ctx.Esi = (uint)Address.Hooks.SendPacket;

                _MainCtx.ctx.Ecx = Memory.ReadUint((uint)Address.Hooks.PacketObject);

                _MainCtx.ctx.Eip = (uint)Address.Hooks.MainLoop;

                SetThreadContext(_MainCtx.Handle, ref _MainCtx.ctx);

            }

           ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);            
        }




    }

}
