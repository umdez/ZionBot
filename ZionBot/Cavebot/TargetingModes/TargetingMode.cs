using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot.TargetingModes
{
    public class TargetingMode : Module
    {
        public Action TargetingAction;

        public TargetingMode()
        {
            this.ThreadEntryPoint = MainLoop;
        }


        void MainLoop()
        {
            if (TargetingAction == null) throw new Exception("Can't execute TargetingMode without TargetingAction being specified.");

            while (Player.IsOnline)
            {
                if (_Cavebot.isGoingToLoot==false && _Cavebot.isLooting==false) TargetingAction();

                Client.Wait(200);

            }
        }





    }
}
