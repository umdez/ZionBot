using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {

        static void ExecuteWalkTo(DEBUG_EVENT evt)
        {
            if (Player.IsOnline)
            {
                Memory.WriteUint(_MainCtx.ctx.Esp, _MainCtx.ctx.Esp + 4); // Address of ESP + 4 at ESP
                LocationToGo.WriteLocation(_MainCtx.ctx.Esp + 4); // Write X on ESP4, Y on ESP8 and Z on ESPC



                _MainCtx.ctx.Esi = (uint)Address.Hooks.WalkTo;

                _MainCtx.ctx.Ecx = Memory.ReadUint((uint)Address.Hooks.WalkObject);

                _MainCtx.ctx.Eip = (uint)Address.Hooks.MainLoop;

                SetThreadContext(_MainCtx.Handle, ref _MainCtx.ctx);
            }

            ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);


        }






        static Location LocationToGo = new Location();
        public static void WalkTo(Location loc)
        {
                Commands.Enqueue(Command.WalkTo);
                LocationToGo = loc;
        }













    }
}
