using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {


        static Breakpoint brGetGuiObject;
        static Breakpoint.BreakPointHandler OnGetGuiObject;


        static void LoadGetGuiObjectBreakpoint(bool StartOnRun)
        {

            OnGetGuiObject = GetGuiObjectCallBack;
            brGetGuiObject = new Breakpoint((uint)Address.Hooks.GetGuiObject, OnGetGuiObject, StartOnRun);
        }


        public static uint GuiObject = 0;

        static void GetGuiObjectCallBack(DEBUG_EVENT evt)
        {
            GetCtx();

            GuiObject = ctx.Edi;

            ContinueBreakPoint(brGetGuiObject, false);
        }


        public static void GetGuiObject()
        {
            brGetGuiObject.Activated = true;
        }


    }
}
