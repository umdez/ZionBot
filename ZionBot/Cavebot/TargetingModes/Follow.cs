using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot.TargetingModes
{
    public class Follow : TargetingMode
    {

        
        public Follow() : base()
        {
            this.AtStart += () => { if (_Cavebot.isGoingToLoot == false && _Cavebot.isLooting == false) Player.AttackAndFollow(); };
            this.TargetingAction = (() =>
            {
                if (Player.isAttacking && !Player.isWalking && !Player.AttackingCreature.Location.IsAdjacentTo(Player.Location))
                {
                    Player.AttackAndFollow();
                    Client.Wait(500);
                }

                Client.Wait(100);
            });
        }





    }
}
