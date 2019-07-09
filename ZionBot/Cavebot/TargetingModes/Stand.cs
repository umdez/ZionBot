using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot.TargetingModes
{
    public class Stand : TargetingMode
    {


        public Stand() : base()
        {
            this.TargetingAction = (() =>
            {
               
                Player.AttackAndStand();

                Client.Wait(2000);
            });
        }






    }
}
