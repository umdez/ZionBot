using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
//using System.Windows;
using IronPythonConsole;
using System.Threading;

namespace OtClientBot
{
    static class Program
    {
        //public static Process TargetProcess;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 


        static bool attachDebugger = true;

        public static MainForm mainForm;





        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var ClientSelector = new ClientSelector();

            if (DialogResult.OK == ClientSelector.ShowDialog())
            {
                if (SetupEverything())
                {
                    mainForm = new MainForm();
                    Application.Run(mainForm);
                    //Application.Run(new TestForm());
                    //Application.Run(new Lua.LuaConsole());
                }
            }

            CleanUpEverything();
        }

        private static void CleanUpEverything()
        {
            Breakpoint.RemoveAllBreakPoints();

            if (Utilities.LogWritter.WritterThread != null) Utilities.LogWritter.WritterThread.Abort();
        }

        private static bool SetupEverything()
        {
            //WinApi.FreeConsole();
            //WinApi.AllocConsole();

            Utilities.LogWritter.Start();

            if (attachDebugger) Debugger.Start(Client.process);


            Memory.TargetHandle = Client.process.Handle;
            Post.hWindow = Client.process.MainWindowHandle;


            Client.SetLight(16);




            Client.PacketAddress = 
                (uint)WinApi.VirtualAllocEx(Client.process.Handle, (IntPtr)0, 2048, WinApi.MEM_COMMIT | WinApi.MEM_RESERVE, WinApi.PAGE_EXECUTE_READWRITE);


            Memory.WriteBytes(Client.PacketAddress, Utils.StringToByteArrayFastest(Address.Client.packetHeader));
                      



            return true;
        }


        


        internal static void Log(string v, bool SameLine = false)
        {
            Utilities.LogWritter.Logs.Enqueue(v + (SameLine ? "" : "\n"));
        }

    }
}
