using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public partial class Debugger
    {

        static Queue<Command> Commands = new Queue<Command>();

        public enum Command
        {
            SendPacket,
            WalkTo,
            Deattach,
            SetAttackingCreature,
            RecievePacket
        }

        public static bool isCommandEngineReady = true;
        static void ExecuteCommands()
        {

            if (ThereIsCommand()) // If there is commands, execute command
            {
                if (isCommandEngineReady)
                {
                    brMainLoop.Activated = true;
                    System.Threading.Thread.Sleep(1);
                }               
            }
        }

        


        private static bool ThereIsCommand()
        {
            return (Commands.Count > 0);
        }















    }
}
