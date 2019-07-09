using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot
{
    public class Walker : Module
    {
        public List<Waypoint> Waypoints;


        private int _currentWaypointIndex = 0;
        public int CurrentWaypointIndex
        {
            get
            {
                return _currentWaypointIndex;
            }
            set
            {
                _currentWaypointIndex = value;
                _currentWaypointIndex %= this.Waypoints.Count;
            }
        }
        Waypoint CurrentWayPoint
        {
            get
            {
                return Waypoints[CurrentWaypointIndex];
            }
        }

        private void Load()
        {
            this.ThreadEntryPoint = WalkLoop;
            //AtStop += new Action(() => Player.WalkTo(Player.Location));
            //AtPause += new Action(() => Player.WalkTo(Player.Location));
        }

        public Walker(List<Waypoint> Waypoints)
        {
            this.Waypoints = Waypoints;
            Load();

        }

        public Walker(Waypoint[] Waypoints)
        {
            this.Waypoints = Waypoints.ToList();
            Load();
        }

        public void WalkLoop()
        {

            while (Player.IsOnline)
            {
                Program.mainForm.UpdateWalkerView();
                CurrentWayPoint.ExecutionCount += 1;
                if (ExecuteWaypoint(CurrentWayPoint)) CurrentWayPoint.SuccessCount += 1;
                else
                {
                    Thread.Sleep(500);
                    CurrentWayPoint.ErrorCount += 1;
                    Program.Log("Error at Index: " + CurrentWaypointIndex.ToString() + " in: " + CurrentWayPoint.ToString());
                }
                CurrentWaypointIndex += 1;
                Thread.Sleep(200);
            }




        }


        bool ExecuteWaypoint(Waypoint wp)
        {
            SpinWait.SpinUntil( () => _Cavebot.canExecuteWalker());

            Program.Log("Cavebot: Executing next waypoint: " + wp.ToString());
            //Program.Log("There are targets on screen:" + _Cavebot.thereAreTargetsOnScreen.ToString() );
            //Program.Log("Is going to loot:" + _Cavebot.isGoingToLoot.ToString());


            _Cavebot.thereAreTargetsOnScreen = true;

            if (Player.Location.Z != wp.Location.Z) return false;

            switch (wp.Action)
            {
                case Enums.Action.Walk:
                    return ExecuteWalk(wp);
                case Enums.Action.Stand:
                    return ExecuteStand(wp);
                case Enums.Action.Use:
                    return ExecuteUse(wp);
                case Enums.Action.UseItemOn:
                    return ExecuteUseItemOn(wp);
                case Enums.Action.Rope:
                    return ExecuteUseRope(wp);
                case Enums.Action.Shovel:
                    return ExecuteUseShovel(wp);

            }





            return true;
        }

        private bool ExecuteUseItemOn(Waypoint wp,bool RemoveJunkFromTop = true,bool StayAdjacentWhenUsing = true)
        {
            ushort itemId;
            if (!UInt16.TryParse(wp.Argument,out itemId))
            {
                return false;
            }

            if (!ExecuteWalk(wp))
            {
                return false;
            }


            // Remove all things that are on top of the tile. 
            if (RemoveJunkFromTop)
            {
                Player.RemoveAllItemsFrom(wp.Location);
            }

            // Walk away from the spot to use the item
            if (StayAdjacentWhenUsing)
            {
                if (Player.Location.IsAdjacentTo(wp.Location, 0))
                {

                    Location walkTo;
                    Player.Location.GetWalkableLocationAround(1, out walkTo,false,true);
                    walkTo.WalkTo();
                    do
                    {
                        Client.WaitPing();
                    } while (Player.isWalking);

                }
            }


            Item item = Iventory.FindItem(itemId);

            if (item == null) return false;

            Client.LastStatusBarMessage = "";

            Player.UseWith(item, wp.Location);

            Client.WaitPing();

            if ( Client.LastStatusBarMessage == Enums.stdMessages.YouCannotUseThisObject ) return false;

            return true;



        }


        public bool ExecuteUseShovel(Waypoint wp)
        {
            wp.NodeRadius = 1;
            wp.Argument = ((ushort)Items.Tools.Shovel).ToString();
            
            if (!ExecuteUseItemOn(wp)) return false;

            return true;


        }

        public bool ExecuteUseRope(Waypoint wp)
        {
            wp.NodeRadius = 1;
            wp.Argument = ((ushort)Items.Tools.Rope).ToString();

            if (!ExecuteUseItemOn(wp)) return false;

            // If player did not rope
            if (Player.Z == wp.Location.Z) return false;

            return true;


        }

        public bool ExecuteUse(Waypoint wp)
        {
            int c = 0;
            bool repeat;
            wp.NodeRadius = 1;
            if (!ExecuteWalk(wp))
            {
                return false;
            }

            if (!Minimap.GetTile(wp.Location).isStairs()) Player.RemoveAllItemsFrom(wp.Location);

            do
            {
                SpinWait.SpinUntil(() => _Cavebot.canExecuteWalker());
                _Cavebot.thereAreTargetsOnScreen = true;



                repeat = false;
                Client.LastStatusBarMessage = "";

                Player.UseGround(wp.Location);

                Client.WaitPing(500);

                repeat |= (Client.LastStatusBarMessage == Enums.stdMessages.YouCannotUseThisObject);

                bool isLadder = Minimap.GetTile(wp.Location).isStairs();
                isLadder |= Minimap.GetTile(new Location(wp.Location.X, wp.Location.Y, wp.Location.Z - 1)).isStairs();
                isLadder |= Minimap.GetTile(new Location(wp.Location.X, wp.Location.Y, wp.Location.Z + 1)).isStairs();

                if (Player.Location.Z == wp.Location.Z && isLadder) repeat |= true;
                c++;

            } while (repeat && c<5);

            return c < 5;




        }


        public bool ExecuteStand(Waypoint wp)
        {
            wp.NodeRadius = 0;
            return ExecuteWalk(wp);
        }


        int NumberOfLoops = 10;
        public bool ExecuteWalk(Waypoint wp, int MaxLoops = 3)
        {
            if (Player.Location.IsAdjacentTo(wp.Location, wp.NodeRadius))
            {
                return true;
            }

            if (Player.isWalking)
            {
                Post.Esc();

                while (Player.isWalking)
                {
                    Thread.Sleep(100);
                }

            }



            Location goalLocation = new Location(wp.Location.X, wp.Location.Y, wp.Location.Z);

            int loops = 0;
            bool first = true;
            while (true)
            {
                SpinWait.SpinUntil(() => _Cavebot.canExecuteWalker());
                _Cavebot.thereAreTargetsOnScreen = true;



                if (loops >= NumberOfLoops) {
                    Program.Log(string.Format("Waypoint {0} returned false due to {1} loops.", CurrentWayPoint.ToString(),NumberOfLoops));
                    return false;
                }

                if (Player.Z != wp.Location.Z) return (true ^ first); // Exclusive Or, this is. If it is first, it will return false, if not it will return true.

                first = false;
                        
                // Here we handle cases where the specific point is blocked by creatures or items since we can walk to any point on the node ( Find Walkable tiles inside Node radius and select the closest one. )

                if(!wp.Location.isWalkable())
                {
                    // We out goalLocation and use it to WalkTo so we do not overwritte wp.Location
                    if(!wp.Location.GetWalkableLocationAround(wp.NodeRadius,out goalLocation))
                    {
                        return false;
                    }
                }

                // And also we need to handle parcels and boxes on the way that increases the draw_elevation making the map and make the tile unpatheable. ( A* )
                // This is done inside the Player.WalkTo Method by using Zion Path Finder if the client PathFinder could not find the path due to a parcel.
                if (!Player.WalkTo(goalLocation)) return false;

 

                do
                {
                    if (Player.Location.IsAdjacentTo(wp.Location, wp.NodeRadius))
                    {
                        return true;
                    }
                    Thread.Sleep(100);

                } while (Player.isWalking);

                if (Player.Z != wp.Location.Z) return (true ^ first);

                //Program.Log("Player stopped walking but it's not on the goal location yet.");



                if (loops > 6)
                {
                    Post.Esc();
                    do
                    {
                        Thread.Sleep(300);
                    } while (Player.isWalking);
                }
                Client.WaitPing(100);

                loops += 1;


            }
        }














    }
}
