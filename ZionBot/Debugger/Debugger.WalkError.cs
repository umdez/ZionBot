using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {


        static Breakpoint brWalkError;
        static Breakpoint.BreakPointHandler OnWalkError;


        static void LoadWalkErrorBreakpoint(bool StartOnRun)
        {

            OnWalkError = WalkErrorCallBack;
            brWalkError = new Breakpoint((uint)Address.Hooks.WalkError, OnWalkError, StartOnRun);
        }



        static bool Success = true;

        static void WalkErrorCallBack(DEBUG_EVENT evt)
        {
            Success = false;
            ContinueBreakPoint(brWalkError, false);
        }


        public static void ActiveWalkError()
        {
            Success = true;
            brWalkError.Activated = true;
        }

        public static void DeactiveWalkError()
        {
            brWalkError.Activated = false;
        }

        public static bool WalkSucceded
        {
            get { return Success; }
        } 

    }
}
