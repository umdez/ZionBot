using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot
{
    public class _Cavebot
    {
        public static Walker Walker;
        public static Targeting Targeting;
        public static AutoLoot AutoLoot;

        private static bool _isGoingToLoot = false;

        public static bool isGoingToLoot { get { return _isGoingToLoot; } set { if (_isGoingToLoot != value) Program.Log("IsGoingToLoot set to:" + value.ToString()); _isGoingToLoot = value;  } }

        public static bool _thereAreTargetsOnScreen = false;


        public static bool thereAreTargetsOnScreen {
            get
            {
                if (Player.Z != PlayerLastZ) { PlayerLastZ = Player.Z ; return true; }
                return _thereAreTargetsOnScreen;
            }
            set {/* if (_thereAreTargetsOnScreen != value) Program.Log("thereAreTargetsOnScreen set to:" + value.ToString());*/ _thereAreTargetsOnScreen = value; } }

        public static bool isWalkerRunning { get { if (Walker != null && Walker.isAlive()) return true; else return false; } }

        public static bool isTargetingRunning { get { if (Targeting != null && Targeting.isAlive()) return true; else return false; } }

        public static bool isTargetingModeRunning { get { if (Targeting != null && Targeting.targetingMode != null && Targeting.targetingMode.isAlive()) return true; else return false; } }


        public static bool isAutoLootRunning { get { if (AutoLoot != null && AutoLoot.isAlive()) return true; else return false; } }

        // This is used to ensure that we always check if there are new targets when we change floors.
        public static byte PlayerLastZ = 0;

        internal static bool isLooting
        {
            get
            {
                if (AutoLoot != null)
                {
                    return AutoLoot.isLooting;
                }
                return false;
            }
        }

        

        internal static bool canExecuteWalker()
        {
            bool isLooting = false;
            bool isTargeting = false;
            int c = 0;

            while (c < 2)
            {
                Client.Wait(50);

                isLooting |= isAutoLootRunning && (isGoingToLoot || _Cavebot.isLooting);

                isTargeting |= isTargetingRunning && thereAreTargetsOnScreen;

                if (isLooting || isTargeting) return false;

                c++;
            }


            return true;

        }


    }
}
