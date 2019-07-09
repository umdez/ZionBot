using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {
        static Queue<Packet> RecievePackets = new Queue<Packet>();
        


        public static void RecievePacket(Packet p)
        {
            RecievePackets.Enqueue(p);
            Commands.Enqueue(Command.RecievePacket);
        }

        private static Packet PacketToRecieve;

        private static void ExecuteRecievePacket(DEBUG_EVENT evt)
        {
            if (RecievePackets.Count > 0)
            {
                PacketToRecieve = RecievePackets.Dequeue(); // Get Packet


                // Modify the context so the instruction "call esi" will actually call our function

                PacketToRecieve.WriteIncomingPacket(Client.RecievePacketAddress);


                Memory.WriteUint(_MainCtx.ctx.Esp + 4, Client.RecievePacketAddress); // Mov [ESP + 4] , PacketAddress
                Memory.WriteUint(_MainCtx.ctx.Esp, _MainCtx.ctx.Esp + 4);          // Move [ESP] , ESP + 4


                _MainCtx.ctx.Esi = (uint)Address.Hooks.RecievePacket;

                _MainCtx.ctx.Ecx = Memory.ReadUint((uint)Address.Hooks.ParsePacketObject);

                _MainCtx.ctx.Eip = (uint)Address.Hooks.MainLoop;

                SetThreadContext(_MainCtx.Handle, ref _MainCtx.ctx);

            }

           ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);            
        }




    }

}
