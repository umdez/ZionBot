using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public partial class Debugger
    {

        const int DBG_CONTINUE = 0x00010002;
        const int DBG_EXCEPTION_NOT_HANDLED = unchecked((int)0x80010001);
        const int CREATE_SUSPENDED = 0x4;
        const int FULL_THREAD_ACESS = 0x1FFFFF;
        const uint EXCEPTION_BREAKPOINT = 0x80000003;
        public const uint THREAD_NORMAL_CONTEXT_FLAG = 65559;


        static byte[] useWithPacket = new byte[] { 0x83, 0xFF, 0xFF, 00, 00, 00, 0x66, 0x66, 00, 0, 0, 0, 0, 0, 0, 0, 0 };



        public enum DebugEventType : uint
        {
            CREATE_PROCESS_DEBUG_EVENT = 3, //Reports a create-process debugging event. The value of u.CreateProcessInfo specifies a CREATE_PROCESS_DEBUG_INFO structure.
            CREATE_THREAD_DEBUG_EVENT = 2, //Reports a create-thread debugging event. The value of u.CreateThread specifies a CREATE_THREAD_DEBUG_INFO structure.
            EXCEPTION_DEBUG_EVENT = 1, //Reports an exception debugging event. The value of u.Exception specifies an EXCEPTION_DEBUG_INFO structure.
            EXIT_PROCESS_DEBUG_EVENT = 5, //Reports an exit-process debugging event. The value of u.ExitProcess specifies an EXIT_PROCESS_DEBUG_INFO structure.
            EXIT_THREAD_DEBUG_EVENT = 4, //Reports an exit-thread debugging event. The value of u.ExitThread specifies an EXIT_THREAD_DEBUG_INFO structure.
            LOAD_DLL_DEBUG_EVENT = 6, //Reports a load-dynamic-link-library (DLL) debugging event. The value of u.LoadDll specifies a LOAD_DLL_DEBUG_INFO structure.
            OUTPUT_DEBUG_STRING_EVENT = 8, //Reports an output-debugging-string debugging event. The value of u.DebugString specifies an OUTPUT_DEBUG_STRING_INFO structure.
            RIP_EVENT = 9, //Reports a RIP-debugging event   (system debugging error). The value of u.RipInfo specifies a RIP_INFO structure.
            UNLOAD_DLL_DEBUG_EVENT = 7, //Reports an unload-DLL debugging event. The value of u.UnloadDll specifies an UNLOAD_DLL_DEBUG_INFO structure.
        }

        public enum HookRegister
        {
            None = 0,
            DR0 = 1,
            DR1 = 2,
            DR2 = 4,
            DR3 = 8
        }



        private static string GetEventName(int code)
        {
            switch (code)
            {
                case 1:
                    return "EXCEPTION_DEBUG_EVENT";
                case 2:
                    return "CREATE_THREAD_DEBUG_EVENT";
                case 3:
                    return "CREATE_PROCESS_DEBUG_EVENT";
                case 4:
                    return "EXIT_THREAD_DEBUG_EVENT";
                case 5:
                    return "EXIT_PROCESS_DEBUG_EVENT";
                case 6:
                    return "LOAD_DLL_DEBUG_EVENT";
                case 7:
                    return "UNLOAD_DLL_DEBUG_EVENT";
                case 8:
                    return "OUTPUT_DEBUG_STRING_EVENT";
                default:
                    return code.ToString();
            }


        }


    }
}
