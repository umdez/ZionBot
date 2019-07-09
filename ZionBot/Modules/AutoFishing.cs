using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static OtClientBot.Minimap;

namespace OtClientBot
{
    public class AutoFishing : Module
    {

        public static string ABC = "123";

        static List<Location> WaterTiles = new List<Location>();

        Location PointOfView = new Location();

        private const byte WaterMapColor = 40;

        int UpdateDelay = 500;

        public bool Activated = false;

        public int FishingDelay = 2000;

        private ushort FishingRodId = 2580;
        private ushort WaterId = 490;

        public AutoFishing()
        {
            base.ThreadEntryPoint = FishingThread;           
            base.AtStart+= Setup;
        }



        void FishingThread()
        {            
            while (Player.IsOnline)
            {
                if (Player.Location.ToString() == PointOfView.ToString())
                {
                    UseFishingRodOnWater();
                }
                else
                {
                    UpdatePointOfView();
                }
            Thread.Sleep(20);
            }
        }

        private void Setup()
        {
            PointOfView = Player.Location;
            WaterTiles = GetWaterTiles();
        }

        private void UpdatePointOfView(bool ini = false)
        {
            bool Differ = true;
            do
            {
                PointOfView = Player.Location;

                if (!ini) Thread.Sleep(UpdateDelay);


                Differ = PointOfView.ToString() != Player.Location.ToString();

            } while (Differ);      

            WaterTiles = GetWaterTiles();

        }

        private List<Location> GetWaterTiles()
        {
            var WaterTilesList = new List<Location>();
            int StartX = PointOfView.X - 7;
            int StartY = PointOfView.Y - 5;

            for (int yOffset = 0; yOffset<11;yOffset++)
            {
                for (int xOffset = 0; xOffset < 15; xOffset++)
                {
                    int CurrentX = StartX + xOffset;
                    int CurrentY = StartY + yOffset;

                    if (isWater(CurrentX,CurrentY))
                    {
                        WaterTilesList.Add(new Location(CurrentX, CurrentY, Player.Z));
                    }
                }
            }

            return WaterTilesList;



        }

        public bool isWater(int x, int y)
        {
            int iniX = x - 1;
            int iniY = y - 1;

            for(int i = 0; i<3;i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    x = iniX + i;
                    y = iniY + k;
                    if (GetTileColor(x, y)!=WaterMapColor) return false;
                }
            }

            return true;


        }

        private void UseFishingRodOnWater()
        {
            if ( WaterTiles.Count < 1)
            {
                Client.Output("No water tiles were found. Stopping auto fishing.");
                Stop();
                return;
            }

            
            Item FishingRod = Iventory.FindItem(FishingRodId);

            if (FishingRod == null)
            {
                Client.Output("Fishing rod was not found. Stopping auto fishing.");
                Stop();
                return;
            }


            int randNumber = new Random().Next(0, WaterTiles.Count - 1);

            Location RandomWaterLoc = WaterTiles[randNumber];
           

            string DifX = (Player.X - RandomWaterLoc.X).ToString();
            string DifY = (Player.Y - RandomWaterLoc.Y).ToString();

            Console.WriteLine(RandomWaterLoc.ToString() + "  {" + DifX + " , " + DifY + "}"  );


            Player.UseWith(FishingRod, RandomWaterLoc,WaterId);

            Thread.Sleep(FishingDelay);


        }
        

    }
}
