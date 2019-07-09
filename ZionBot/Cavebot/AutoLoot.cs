using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot
{
    public class AutoLoot : Module
    {

        public bool LootAll = false;

        public Queue<LootPoint> LootQueue;

        public List<LootItem> LootList;

        public bool EatFoods = true;


        public bool isLooting = false;




        public struct LootItem
        {
            public uint id;
            public byte? bp;

        }


        public struct LootPoint
        {
            public Location location;
            public uint corposeId;
        }


        private void Load()
        {
            this.LootQueue = new Queue<LootPoint>();
            this.ThreadEntryPoint = LootingLoop;
            this.AtStop += new Action(()=> { _Cavebot.isGoingToLoot = false;  Debugger.LootAll = false; Debugger.LootQueue = null; });
            this.AtStart += new Action(() => { _Cavebot.isGoingToLoot = false;  Debugger.LootAll = this.LootAll; Debugger.LootQueue = this.LootQueue; });
        }

        public AutoLoot(List<LootItem> LootList, bool LootAll)
        {
            this.LootList = LootList;
            this.LootAll = LootAll;
            Load();
        }


        public AutoLoot(List<ushort> ItemsIds, bool LootAll)
        {
            this.LootList = new List<LootItem>();

            foreach (ushort itemId in ItemsIds) this.LootList.Add(new LootItem() { id = itemId });

            this.LootAll = LootAll;
            Load();
        }

        public AutoLoot()
        {
            this.LootList = new List<LootItem>();
            LootList.Add(new LootItem() { id = 2148 });
            Load();
        }


        public enum AutoLootResult : byte
        {
            Success = 0,
            DidNotOpenCorpose = 1,
            isAtStairs = 2,
            NotPossible = 4,
            DidNotReachCorpose = 8
            
        }




        private void LootCorpose(byte corposeIndex, bool LootInsideBags = true, List<byte> Except = null)
        {
            Program.Log("AutoLoot: Starting to pick up items... ");
            Container corpose = Iventory.GetContainerByIndex(corposeIndex);

            if (corpose == null)
            {
                Program.Log("AutoLoot at \"Loot Corpose\": Corpose was null.");
                return;
            }

            // Adiciona a excessão. 
            if (Except == null) Except = new List<byte>();

            Except.Add(corposeIndex);



            // Take all specified items by moving them from the last to the first.
            foreach (Item item in (from item in corpose.Items orderby item.Location.Z descending select item))
            {
                var Matches = (from x in LootList where x.id == item.Id select x.bp);

                if (Matches.Count() == 0) continue;

                Program.Log("AutoLoot: Found " + item.Id.ToString());

                byte ? bp = Matches.FirstOrDefault();


                Location moveTo = null;
                Container BackPack = null;

                if (bp != null)
                {
                    BackPack = Iventory.GetContainerByIndex((int)bp);

                    moveTo = BackPack.GetLootSlot(item);
                }
            
                // If could not get backpack at specified index, then try any empty slot.
                if (moveTo == null)
                {
                    
                    moveTo = Iventory.FindLootSlot(item,Except: Except);

                    BackPack = Iventory.GetContainerByIndex((int)(moveTo.Y - Container.ContainerYStart));
                }


                if (moveTo == null) return;

                Program.Log($"AutoLoot: Looting {item.Count} {item.Id}");

                Player.MoveItem(item, moveTo, item.Count );

                Client.Wait(100);

                if ( BackPack.Capacity - BackPack.ItemCount < 2)
                {
                    BackPack.StackItems();
                }
                

                }


            if (LootInsideBags)
            {
                foreach(Item bag in (from item in corpose.Items where OtClientBot.Items.Containers.Contains(item.Id) select item))
                {

                    byte lootBagIndex = Iventory.EmptyIndex;

                    Client.LastStatusBarMessage = "";

                    Player.Open(bag, lootBagIndex);

                    // Wait until the container is Open...

                    for (int i = 0; i <= 100; i++)
                    {
                        if (Client.LastStatusBarMessage == Enums.stdMessages.YouCannotUseThisObject) return;

                        if (Iventory.EmptyIndex != lootBagIndex) break;

                        Client.Wait(10);
                    }

                    if (Iventory.EmptyIndex == lootBagIndex) return;

                    // Pega los items
                    LootCorpose(lootBagIndex,false);


                    // Fexa el container
                    Iventory.CloseContainer(lootBagIndex);


                    // Wait until the container is Closed...

                    for (int i = 0; i <= 100; i++)
                    {
                        if (Iventory.EmptyIndex == lootBagIndex) break;

                        Client.Wait(10);
                    }




                }
            }



            if (this.EatFoods)
            {

                foreach (Item food in (from item in corpose.Items where Items.Foods.Contains(item.Id) orderby item.Location.Z descending select item))
                {
                    int times = 0;
                    while (times < food.Count)
                    {
                        Player.UseItem(food);
                        Client.Wait(50);
                        times++;
                    }
                }

            }





            }











       

        private AutoLootResult LootIt(LootPoint point)
        {
            Program.Log("AutoLoot: Going to Loot at:" + point.location.ToString());
            bool previousMode = Player.FollowMode == 1;

            if (Player.isWalking) Player.Stop();

            Player.SetFollowMode(false);


            // If location is stairs, move the body and then use it. but for now, let's just skip it
            if (point.location.isStairs()) {

                Program.Log("AutoLoot: Corpose is at stairs. Skipping it.") ;
                return AutoLootResult.isAtStairs;
            }


            if (Player.Location.IsAdjacentTo(point.location) == false)
            {

                if (point.location.isReachable() == false) return AutoLootResult.NotPossible;

                Program.Log("AutoLoot: Location is Reachable.");

                // Tries to reach the corpose five times
                for (int i = 1; i <= 5; i++)
                {
                    if (Player.Location.IsAdjacentTo(point.location)) break;

                    Program.Log("AutoLoot: Walking to Location.");

                    Location gotoLocation;

                    if (!point.location.GetWalkableLocationAround(1, out gotoLocation)) return AutoLootResult.NotPossible;

                    if (!Player.WalkTo(gotoLocation) && (point.location.isReachable() == false)) return AutoLootResult.NotPossible;

                    // Wait until player stops walking
                    for (int j = 0; j <= 300; j++)
                    {
                        if (!Player.isWalking) break;

                        Client.Wait(10);
                    }
                }

                if (Player.Location.IsAdjacentTo(point.location) == false) return AutoLootResult.DidNotReachCorpose;


                Program.Log("AutoLoot: Successfully reached Location");
            }
            else
            {
                Client.WaitPing(1000);
            }

            

            // Alright. If we are adjacent to the corpose now we can open it.

            // Removed this because i was having problems with the corpose ids. Needs to do some tests to understand it.
            //if (point.corposeId!=0 && GameMap.GetTile(point.location).GetTopItem().Id != point.corposeId) return;


            byte corposeIndex = Iventory.EmptyIndex;

            Client.LastStatusBarMessage = "";

            Program.Log("AutoLoot: Opening corpose...");

            Player.UseGround(point.location, corposeIndex);

            // Wait until the container is Open...

            for (int i = 0; i <= 200; i++)
            {
                if (Client.LastStatusBarMessage == Enums.stdMessages.YouCannotUseThisObject)
                {
                    Program.Log("AutoLoot: Can't use this object...");
                    return AutoLootResult.NotPossible;
                }

                if (Iventory.EmptyIndex != corposeIndex) break;

                Client.Wait(10);
            }

            if (Iventory.EmptyIndex == corposeIndex)
            {
                Program.Log("AutoLoot: We sent the packet but the corpose did not open.");
                return AutoLootResult.DidNotOpenCorpose;
            }

            Program.Log("AutoLoot: Successfully opened it.");


            // Pega los items
            LootCorpose(corposeIndex);





            // Fexa el container
            Iventory.CloseContainer(corposeIndex);



            // Wait until the container is Closed...

            for (int i = 0; i <= 100; i++)
            {
                if (Iventory.EmptyIndex == corposeIndex) break;

                Client.Wait(10);
            }



            Player.SetFollowMode(previousMode);


            return AutoLootResult.Success;

        }


        private void LootingLoop()
        {
            while (Player.IsOnline)
            {

                if (LootQueue.Count > 0)
                {
                    isLooting = true;

                    
                    Program.Log("AutoLoot: Taking one of " + LootQueue.Count.ToString() + " LootPoints on the queue.");

                    LootPoint pointToLoot = LootQueue.Dequeue();

                    for (int i = 0; i < 1; i++)
                    {
                        bool tryToLootAgain = true;
                        switch (LootIt(pointToLoot))
                        {
                            case AutoLootResult.Success:
                            case AutoLootResult.NotPossible:
                                tryToLootAgain = false;
                                break;
                            default:
                                break;
                        }

                        if (!tryToLootAgain) break;

                        //else
                        //{
                        //    Program.Log("AutoLoot: Trying to loot again");
                        //}

                    }



                    Iventory.StackItems();

                    this.isLooting = false;


                    if (LootQueue.Count == 0) _Cavebot.isGoingToLoot = false; // This must be inside the if

                }

                Client.Wait(200);
                _Cavebot.isGoingToLoot = false;

            }
        }

    }
}
