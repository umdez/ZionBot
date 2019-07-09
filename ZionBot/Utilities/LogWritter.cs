using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace OtClientBot.Utilities
{
    public static class LogWritter
    {
        public static Queue<string> SentPackets = new Queue<string>();

        public static Queue<string> RecievedPackets = new Queue<string>();

        public static Queue<string> Logs = new Queue<string>();

        public static Thread WritterThread;

        private static string SentPacketsFile = "SentPackets.txt";
        private static string RecievedPacktesFile = "RecievedPackets.txt";


        public static void Start()
        {
            System.IO.File.WriteAllText(SentPacketsFile,"");
            System.IO.File.WriteAllText(RecievedPacktesFile, "");



            WritterThread = new Thread(LogWritterLoop);
            WritterThread.Start();
        }



        public static void LogWritterLoop()
        {

            while (true)
            {
                if (SentPackets.Count>0)
                {
                    StringBuilder Log = new StringBuilder();
                    while (SentPackets.Count > 0) {
                        Log.Append(SentPackets.Dequeue() + "\n");
                    }
                    System.IO.File.AppendAllText(SentPacketsFile,Log.ToString() );                    
                }


                if (RecievedPackets.Count > 0)
                {
                    StringBuilder Log = new StringBuilder();
                    while (RecievedPackets.Count > 0)
                    {
                        Log.Append(RecievedPackets.Dequeue() + "\n");
                    }
                    System.IO.File.AppendAllText(RecievedPacktesFile, Log.ToString());
                }

                if(Program.mainForm!=null && Logs.Count>0)
                {
                    Program.mainForm.WriteLogs();
                }




                Thread.Sleep(500);
            }






        }






    }
}
