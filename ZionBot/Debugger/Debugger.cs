using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {
        private static Process process;
        private static Thread dbgThread;

        private static ThreadContext ctx;
        private static DEBUG_EVENT evt;

        private static Action OnBreakPointsLoad;



        public static void Start(Process process)
        {
            Debugger.process = process;

            
            // Start a thread to perform the debug loop
            dbgThread = new Thread(DebuggerThread) { IsBackground = true };
            dbgThread.Start();
            
        }

        private static void LoadBreakPoints()
        {
            //DebugBreakProcess(process.Handle); // Suspend process
            //WaitForDebugEvent(out evt, 100); // Handle the breakpoint exception from the previous command



            // Breakpoints

            LoadSendTalkBreakPoint(true);

            LoadSendingPacketBreakPoint(true);

            LoadIncomingPacketBreakPoint(true);

            LoadWalkErrorBreakpoint(false);

            LoadGetGuiObjectBreakpoint(true);

            LoadMainLoopBreakpoint(false);

            LoadOnCreatureDeathBreakpoint(true);




            //ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE); // Continue.

        }


        private static bool DebuggerSetup()
        {
            // Check if process field is empy.
            if (process == null)
            {
                Program.Log("Could not start the engine because process field is null.");
                return false;
            }

            // Check if the target is being debugged.
            
            bool isProcessBeingDebugged = true;
            CheckRemoteDebuggerPresent(process.Handle, out isProcessBeingDebugged);

            if (isProcessBeingDebugged)
            {
                Program.Log("Could not attach the Hook Engine.");
                return false;
            }


            // Attach the Debugger.

            if (!DebugActiveProcess(process.Id))
            {
                Program.Log("Error attaching the Hook Engine.");
                return false;
            }

            // Allows the process to keep running when the debugger is deatached.

            DebugSetProcessKillOnExit(false); // Prevents the debuguee to be closed when debugger is closed.


            // Setup the global fields

            evt = new DEBUG_EVENT();
            ctx = new ThreadContext();
            ctx.ContextFlags = 65559;


            // Loads the breakpoints

            LoadBreakPoints();


            return true;

        }





        private static void DebuggerThread()
        {

            // Setup the Debugger
            if (!DebuggerSetup())
            {
                Program.Log("Could not setup the debugger correctly.");
                return;
            }


            while (true)
            {
                HandleEvents();
                ExecuteCommands();
                Thread.Sleep(1);
            }
        }

        private static void HandleEvents()
        {


            // wait for a debug event
            if (WaitForDebugEvent(out evt, 1))
            {
                HandleEvent(evt);
            }



        }

        private static void HandleEvent(DEBUG_EVENT evt)
        {
            if (IsException(evt)) // if It's an Exception -> that may also be a BreakPoint.
            {
                IntPtr exceptionAddress = evt.Exception.ExceptionRecord.ExceptionAddress;


                // Verify if there is a Breakpoint registered in this address
                
                Breakpoint b = null;
                Breakpoint.Breakpoints.TryGetValue((uint)exceptionAddress, out b);

                if (b != null) // If there is a breakpoint in there, then call the handler and return.
                {
                    b.Handler(evt);                    
                    return;
                }


                uint[] logBlackList = { 0x7624c54f, 0x7717000c, 0x77b2000c, 0x76d5c54f, 0x772e000c, 0x76dbc54f, 0, 0x76f8000c, 0x75b2c54f };

                if (logBlackList.Contains((uint)exceptionAddress) == false)
                {

                    // Prints where the not handled exception has occured.
                    //Program.Log("################### Not Handled Exception at: " + exceptionAddress.ToString("x8"));
                    //Program.Log(evt.Exception.ExceptionRecord.ExceptionCode.ToString());
                    var t_ctx = new ThreadContext();
                    GetThreadContext(OpenThread(FULL_THREAD_ACESS, false, (uint)evt.dwThreadId), ref t_ctx);

                    //Program.Log(string.Format("EAX: {0}\nEBX: {1}\nECX: {2}\nESP: {3}\nEBP: {4}\nESI: {5}\nEDI: {6}\nEIP:", ctx.Eax.ToString("x8"), ctx.Ebx.ToString("x8"), ctx.Ecx.ToString("x8"), ctx.Esp.ToString("x8"), ctx.Ebp.ToString("x8"), ctx.Esi.ToString("x8"), ctx.Edi.ToString("x8"), ctx.Eip.ToString("x8")));
                }

                // Continue with Exception Not Handled Flag
            ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_EXCEPTION_NOT_HANDLED);
            }
            else
            {
                // This code is executed when there is any other kind of debug event.

                //Program.Log(GetEventName((int)evt.dwDebugEventCode) + " - Thread Id: " + evt.dwThreadId.ToString("x4"));

                // continue running the debugee
                ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);
            }


        }

        static private bool GetCtx(DEBUG_EVENT evt)
        {
            var hThread = OpenThread(FULL_THREAD_ACESS, false, (uint)evt.dwThreadId);


            return GetThreadContext(hThread, ref ctx);
        }

        static private bool GetCtx()
        {
            var hThread = OpenThread(FULL_THREAD_ACESS, false, (uint)evt.dwThreadId);


            return GetThreadContext(hThread, ref ctx);
        }

        public static void ContinueBreakPoint(Breakpoint b, bool KeepBreakPoint = true)
        {

            var ctx = new ThreadContext();
            ctx.ContextFlags = THREAD_NORMAL_CONTEXT_FLAG;

            b.Activated = false; // Replace 0xCC with original OpCode


            IntPtr ThreadHandle = OpenThread(FULL_THREAD_ACESS, false, ((uint)evt.dwThreadId));

            GetThreadContext(ThreadHandle, ref ctx); // Get Thread context


            ctx.Eip = ctx.Eip - 1; // Replace EIP with EIP-1 ( Back it by one byte )


            if (KeepBreakPoint)
            {

                ctx.EFlags |= 0x100;  // Put into Single Step Mode.

                SetThreadContext(ThreadHandle, ref ctx);

                ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);



                if (!WaitForDebugEvent(out evt, 3000))
                    throw new Exception("Timeout singlestep.");


                b.Activated = true; // replace OpCode with INT3 instruction again.



                GetThreadContext(ThreadHandle, ref ctx);

                if (KeepBreakPoint) ctx.EFlags &= ~(uint)0x100;  // Remove Single Step Flag.

                SetThreadContext(ThreadHandle, ref ctx);

                ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);
            }
            else
            {

                SetThreadContext(ThreadHandle, ref ctx);

                ContinueDebugEvent(evt.dwProcessId, evt.dwThreadId, DBG_CONTINUE);

            }






        }



        private static void SetDebugRegisters(HookRegister register, IntPtr hookLocation, ref ThreadContext ct, bool remove)
        {
            if (remove)
            {
                uint flagBit = 0;
                switch (register)
                {
                    case HookRegister.DR0:
                        flagBit = 1 << 0;
                        ct.Dr0 = 0;
                        break;
                    case HookRegister.DR1:
                        flagBit = 1 << 2;
                        ct.Dr1 = 0;
                        break;
                    case HookRegister.DR2:
                        flagBit = 1 << 4;
                        ct.Dr2 = 0;
                        break;
                    case HookRegister.DR3:
                        flagBit = 1 << 6;
                        ct.Dr3 = 0;
                        break;
                }
                ct.Dr7 &= ~flagBit;
            }
            else
            {
                switch (register)
                {
                    case HookRegister.DR0:
                        ct.Dr0 = (uint)hookLocation;
                        ct.Dr7 |= 1 << 0;
                        break;
                    case HookRegister.DR1:
                        ct.Dr1 = (uint)hookLocation;
                        ct.Dr7 |= 1 << 2;
                        break;
                    case HookRegister.DR2:
                        ct.Dr2 = (uint)hookLocation;
                        ct.Dr7 |= 1 << 4;
                        break;
                    case HookRegister.DR3:
                        ct.Dr3 = (uint)hookLocation;
                        ct.Dr7 |= 1 << 6;
                        break;
                }
                ct.Dr6 = 0;
            }
        }









        private static bool IsException (DEBUG_EVENT evt)
        {
            if (evt.dwDebugEventCode == (uint)DebugEventType.EXCEPTION_DEBUG_EVENT) // if It's an Exception
            {
                return true;
            }
            return false;
        }

    }
}
