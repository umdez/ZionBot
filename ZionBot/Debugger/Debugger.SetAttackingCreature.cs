using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {



        static void ExecuteSetAttackingCreature(DEBUG_EVENT evt)
        {

            // Modify the context so the instruction "call esi" will actually call our function
            if (Player.IsOnline&&(Player.AttackingCreaturePtr != CreatureToAttackPtr))
            {

                Memory.WriteUint(_MainCtx.ctx.Esp + 4, CreatureToAttackPtr); // Mov [ESP + 4] , CreaturePtr
                Memory.WriteUint(_MainCtx.ctx.Esp, _MainCtx.ctx.Esp + 4);          // Move [ESP] , ESP + 4


                _MainCtx.ctx.Esi = (uint)Address.Hooks.SetAttackingCreature;

                _MainCtx.ctx.Ecx = (uint)Address.Hooks.GameObject;

                _MainCtx.ctx.Eip = (uint)Address.Hooks.MainLoop;

                SetThreadContext(_MainCtx.Handle, ref _MainCtx.ctx);

            }
            ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);


        }






        static uint CreatureToAttackPtr = 0;
        public static void SetAttackingCreature(Creature c)
        {
            if (c.Location.isOnScreen() && (Player.AttackingCreaturePtr != c.CreaturePtr ))
            {
                Commands.Enqueue(Command.SetAttackingCreature);
                CreatureToAttackPtr = c.CreaturePtr;
            }

        }











    }
}



