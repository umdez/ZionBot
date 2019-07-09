using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {


        static Breakpoint brOnCreatureDeath;
        static Breakpoint.BreakPointHandler OnCreatureDeath;


        static void LoadOnCreatureDeathBreakpoint(bool StartOnRun)
        {

            OnCreatureDeath = OnCreatureDeathCallBack;
            brOnCreatureDeath = new Breakpoint((uint)Address.Hooks.OnCreatureDeath, OnCreatureDeath, StartOnRun);
        }


        public static Queue<Cavebot.AutoLoot.LootPoint> LootQueue = null;
        public static bool LootAll = false;

        public static uint LastCreature = 0;

        public static uint QueueDelay = 0;

        static void OnCreatureDeathCallBack(DEBUG_EVENT evt)
        {
            GetCtx();

            if (LootQueue!=null)
            {
                var c = new Creature(Memory.ReadUint(  ctx.Esp + 0x8   ));

                if ( c.Location.isOnScreen() &&  (LootAll || c.Id == Player.AttackingCreature.Id) && (LastCreature== 0 || !(c.Id ==(LastCreature))  )  )
                {
                    Cavebot._Cavebot.isGoingToLoot = true;

                    var cTile = GameMap.GetTile(c.Location);

                    int itemCount = cTile.GetItems().Count;

                    Location lootLocation = new Location((int)c.X, (int)c.Y, (int)c.Z);

                    LastCreature = c.Id;

                    new System.Threading.Tasks.TaskFactory().StartNew(() =>
                   {
                       // For three seconds it will verify if the corpose appeared then it will add the location to the queue.
                       for (int i = 0; i <= 300; i++)
                       {
                           if (cTile.GetItems().Count > itemCount)
                           {
                               Client.Wait((int)QueueDelay *  (  BattleList.NumberOfCreaturesAround() > 1 ? 2 : 1 )   );                       
                                LootQueue.Enqueue(
                                    new Cavebot.AutoLoot.LootPoint()
                                    {
                                        location = lootLocation,
                                        corposeId = (cTile.GetTopItem() == null ? (uint)0 : cTile.GetTopItem().Id)
                                    });
                               
                               break;
                           }
                           Client.Wait(10);
                       }
                   });
                }
            }

            ContinueBreakPoint(brOnCreatureDeath, true);
        }





    }
}
