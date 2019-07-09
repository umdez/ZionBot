using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {
        static Breakpoint brMainLoop;
        static Breakpoint.BreakPointHandler ExecuteCommandCallBack;
        static Breakpoint brMainLoopNextInstruction;
        static Breakpoint.BreakPointHandler RestoreContextCallBack;

        static void LoadMainLoopBreakpoint(bool StartOnRun)
        {

            ExecuteCommandCallBack = MainExecuteCommands;
            RestoreContextCallBack = RestoreContext;

            brMainLoop = new Breakpoint((uint)Address.Hooks.MainLoop, ExecuteCommandCallBack, StartOnRun);

            brMainLoopNextInstruction = new Breakpoint((uint)Address.Hooks.MainLoop + 2, RestoreContextCallBack ,false);

        }



        struct Context
        {
            public uint EAX, EBX, ECX, EDX, EBP, ESP, ESI, EDI, EIP, EFLAGS, ESP0, ESP4, ESP8, ESPC;

            public IntPtr Handle;

            public ThreadContext ctx;

        }
        
        static Context _MainCtx = new Context();




		static void MainExecuteCommands(DEBUG_EVENT evt)
        {
            // If There are commands...

            if (Commands.Count > 0)
            {
                brMainLoop.Activated = false;
                brMainLoopNextInstruction.Activated = true;
                SaveContext(evt);
                isCommandEngineReady = false;

                switch (Commands.Dequeue())
                {
                    case Command.SetAttackingCreature:
                        ExecuteSetAttackingCreature(evt);
                        return;
                    case Command.SendPacket:
                        ExecuteSendPacket(evt);
                        return;
                    case Command.WalkTo:
                        ExecuteWalkTo(evt);
                        return;
                    case Command.RecievePacket:
                        ExecuteRecievePacket(evt);
                        return;
                    default:
                        break;
                }
            }

            brMainLoop.Activated = false;
            ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);
            
        }




        private static void RestoreContext(DEBUG_EVENT evt)
        {
            // Deactives Next instruction Breakpoint

            brMainLoopNextInstruction.Activated = false;


            // Restore CTX

            _MainCtx.Handle = OpenThread(FULL_THREAD_ACESS, false, (uint)evt.dwThreadId);
            GetThreadContext(_MainCtx.Handle, ref _MainCtx.ctx);

            _MainCtx.ctx.Eax = _MainCtx.EAX;
            _MainCtx.ctx.Ebx = _MainCtx.EBX;
            _MainCtx.ctx.Ecx = _MainCtx.ECX;
            _MainCtx.ctx.Edx = _MainCtx.EDX;
            _MainCtx.ctx.Ebp = _MainCtx.EBP;
            _MainCtx.ctx.Esp = _MainCtx.ESP;
            _MainCtx.ctx.Esi = _MainCtx.ESI;
            _MainCtx.ctx.Edi = _MainCtx.EDI;
            _MainCtx.ctx.Eip = (uint)Address.Hooks.MainLoop;
            _MainCtx.ctx.EFlags = _MainCtx.EFLAGS;


            SetThreadContext(_MainCtx.Handle, ref _MainCtx.ctx);

            // Restore Stack

            Memory.WriteUint(_MainCtx.ctx.Esp + 0xC, _MainCtx.ESPC);
            Memory.WriteUint(_MainCtx.ctx.Esp + 8, _MainCtx.ESP8);
            Memory.WriteUint(_MainCtx.ctx.Esp + 4, _MainCtx.ESP4);
            Memory.WriteUint(_MainCtx.ctx.Esp, _MainCtx.ESP0);


            ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);


            isCommandEngineReady = true;
        }

        

        static void SaveContext(DEBUG_EVENT evt)
        {


            // Ctx that will be used to in Gettings and Settings at the flow
            _MainCtx.Handle = OpenThread(FULL_THREAD_ACESS, false, (uint)evt.dwThreadId); // Opens Thread Handle
            _MainCtx.ctx = new ThreadContext();
            _MainCtx.ctx.ContextFlags = 63;
            GetThreadContext(_MainCtx.Handle, ref _MainCtx.ctx);

            // Save Registers:

            _MainCtx.EAX = _MainCtx.ctx.Eax;
            _MainCtx.EBX = _MainCtx.ctx.Ebx;
            _MainCtx.ECX = _MainCtx.ctx.Ecx;
            _MainCtx.EDX = _MainCtx.ctx.Edx;
            _MainCtx.EBP = _MainCtx.ctx.Ebp;
            _MainCtx.ESP = _MainCtx.ctx.Esp;
            _MainCtx.ESI = _MainCtx.ctx.Esi;
            _MainCtx.EDI = _MainCtx.ctx.Edi;
            _MainCtx.EIP = _MainCtx.ctx.Eip;
            _MainCtx.EFLAGS = _MainCtx.ctx.EFlags;




            // Save the stack:

            _MainCtx.ESP0 = Memory.ReadUint(_MainCtx.ctx.Esp);
            _MainCtx.ESP4 = Memory.ReadUint(_MainCtx.ctx.Esp + 4);
            _MainCtx.ESP8 = Memory.ReadUint(_MainCtx.ctx.Esp + 8);
            _MainCtx.ESPC = Memory.ReadUint(_MainCtx.ctx.Esp + 0xC);


        }




    }
}
