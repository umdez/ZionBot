using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class BattleList
    {


   
        static bool CheckIfCreatureIsValid(uint CreaturePtr)
        {
            //var c = new Creature(CreaturePtr);
            //if(string.IsNullOrWhiteSpace(c.Name)==false)
            //Console.WriteLine("[ " + c.Name + " ]");
            //if (c.Location.Z != 255) return true;
            if (Memory.ReadByte(CreaturePtr + 0x14) != 255) return true;

            return false;
        }

        public static List<Creature> GetAllCreatures(bool sameFloor)
        {
            var Cond = new Utils.Condition(CheckIfCreatureIsValid);
            
            Dictionary<uint, uint> KeyPairDictionary = Utils.IterateNewUnordMap(Address.BattleList.KnownCreatures,Cond);

            var cList = new List<Creature>();

           // Program.Log(KeyPairDictionary.Count().ToString());

            try
            { KeyPairDictionary.Remove(Player.Id); }   // Removes the actual player from the list.
            catch { Program.Log("Player id not found on list."); }



            foreach (uint CreaturePtr in KeyPairDictionary.Values)
            {
                var cr = new Creature(CreaturePtr);
                if (sameFloor && /*cr.Z != Player.Z*/ cr.Location.isOnScreenMemory()==false ) continue;
                cList.Add(cr);
            }

            return cList;

        }


        public static int NumberOfCreaturesAround(int MaxRange = 5)
        {
            return (from x in GetAllCreatures(true) where x.Location.IsAdjacentTo(Player.Location, MaxRange) select x).Count();

        }

        public static Creature GetNearCreature(int MaxRange = 5, bool onlyCreatures = false)
        {
            Creature creature = null;

            var cList = GetAllCreatures(true);



            double lastDistance = 20.0;
            foreach (Creature c in cList)
            {
                if (onlyCreatures && c.isCreature() == false) continue; // Skip players and npcs.

                double CurrentDistance = Player.Location.WalkDistanceTo(c.Location);
                if (CurrentDistance < lastDistance && CurrentDistance <= MaxRange)
                {
                    creature = c;
                    lastDistance = CurrentDistance;
                }
            }


            if (creature != null)
            {
                Program.Log("Near creature:");
                creature.Print(); }
            else
                Program.Log("Near creature not Found.");

            return creature;
        }







    }
}
