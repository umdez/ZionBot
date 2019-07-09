using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Location
    {
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public byte Z { get; set; }

        public bool isIventory { get { if (X == 0xFFFF) return true; else return false; } }

        public int container { get { return Y; } }
        public int slot { get { return Z; } }

        private byte[] _uintRaw;
        public byte[] uintRaw
        {
            get
            {
                if (_uintRaw == null)
                {
                    _uintRaw = new byte[12];
                    BitConverter.GetBytes(X).CopyTo(_uintRaw, 0);
                    BitConverter.GetBytes(Y).CopyTo(_uintRaw, 4);
                    BitConverter.GetBytes(Z).CopyTo(_uintRaw, 8);
                }
                return _uintRaw;

            }
        }

        private byte[] _ushortRaw;
        public byte[] ushortRaw
        {
            get
            {
                if (_ushortRaw == null)
                {
                    _ushortRaw = new byte[5];

                    BitConverter.GetBytes(X).CopyTo(_ushortRaw, 0);
                    BitConverter.GetBytes(Y).CopyTo(_ushortRaw, 2);
                    ushortRaw[4] = Z;
                }
                return _ushortRaw;       

            }
        }
                       
        

        public Location()
        {
            X = 0xFFFF;
            Y = 0xFFFF;
            Z = 0xFF;
        }

        public Location(int x, int y, int z)
        {
            X = (ushort)x;
            Y = (ushort)y;
            Z = (byte)z;
        }


        public bool WalkTo()
        {
            return Player.WalkTo(this);
        }

        public void WriteLocation(long Address)
        {
            Memory.WriteBytes(Address, uintRaw);
        }

        public override string ToString()
        {

            return string.Format("X: {0}, Y: {1}, Z: {2}", X, Y, Z);

        }

        public Location addOffset(int x, int y, int z = 0)
        {
            this.X = (ushort)(this.X+ x);
            this.Y = (ushort)(this.Y + y);
            return this;
        }


        public bool isStairs()
        {
            return Minimap.GetTileColor(this) == (byte)Minimap.Colors.Stairs;
        }

        public bool isTheSameAs(Location loc)
        {
            if (this.Z != loc.Z) return false;
            if (this.X != loc.X) return false;
            if (this.Y != loc.Y) return false;            
            return true;
        }

        public bool isOnScreen()
        {
            if (Player.Z != Z) return false;
            if (Math.Abs(Player.Y - Y) > 5) return false;
            if (Math.Abs(Player.X - X) > 7) return false;
            return true;
        }

        public bool isOnScreenMemory()
        {
            if (Player.Z != Z) return false;
            if (Math.Abs(Player.Y - Y) > 6) return false;
            if (Math.Abs(Player.X - X) > 8) return false;
            return true;
        }


        public Point OffsetTo(Location l)
        {
            var res = new Point();

            res.X = l.X - this.X;
            res.Y = l.Y - this.Y;

            return res;
        }

        public double WalkDistanceTo(Location l)
        {
            int xDist = Math.Abs(X - l.X);
            int yDist = Math.Abs(Y - l.Y);

            return xDist+yDist;
        }

        internal bool isReachable(bool ConsiderCreatureAtGoal = true)
        {
            return new Utilities.PathFinder().ThereIsPath(this,ConsiderCreatureAtGoal);          
        }

        public double SquareDistanceTo(Location l)
        {
            int xDist = Math.Abs(X - l.X);
            int yDist = Math.Abs(Y - l.Y);

            return Math.Sqrt(xDist * xDist + yDist * yDist);

        }

        public bool IsAdjacentTo(Location loc, int range = 1)
        {
            if (Math.Max(Math.Abs(X - loc.X), Math.Abs(Y - loc.Y)) <= range && loc.Z == Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public  bool isWalkable(bool ConsiderCreatures = true)
        {
            // Still Needs to check for "non-walkable" things on the tile.  ( Reading things attributes is a requiriment )
            return (Minimap.GetTile(this).isWalkable() && ((this.isOnScreenMemory() && ConsiderCreatures) ? !GameMap.GetTile(this).isTopCreature() : true));
        }

        public bool GetItemThrowableLocationAround(out Location ItemThrowableLocation)
        {
            ItemThrowableLocation = new Location();

            var PlayerOffsetTo = Player.Location.OffsetTo(this);

            var PossibleLocations = new List<Location>();

            for (int x =-1; x <=1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == PlayerOffsetTo.X && y == PlayerOffsetTo.Y) continue;
                    
                    var testLocation = Player.Location.addOffset(x, y);

                    if (Minimap.GetTile(testLocation).isWalkable())
                    {
                        PossibleLocations.Add(testLocation);
                    }

                }
            }

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    var testLocation = new Location(this.X, this.Y, this.Z).addOffset(x, y);

                    if (Minimap.GetTile(testLocation).isWalkable())
                    {
                        PossibleLocations.Add(testLocation);
                    }

                }
            }

            if (PossibleLocations.Count > 0)
            {
                ItemThrowableLocation = PossibleLocations[new Random().Next(0, PossibleLocations.Count)];
                return true;
            }

            return false;


        }

        public bool GetWalkableLocationAround(int Radius, out Location WalkableLocation,bool ConsiderStairs=false,bool ExcludeCenter = false,bool ConsiderCreatures = true)
        {
            WalkableLocation = new Location();


            if (ExcludeCenter && Radius == 0) return false;

            if (Radius == 0)
            {
                return this.isWalkable();
            }



            List<Location> WalkableLocations = new List<Location>();

            for (int x = -Radius; x <= Radius; x++)
            {
                for (int y = -Radius; y <= Radius; y++)
                {
                    Location testLocation = new Location(this.X + x, this.Y + y, this.Z);
                    if (testLocation.isWalkable(ConsiderCreatures))
                    {
                        if (ExcludeCenter && x == 0 && y == 0) continue;

                        if (ConsiderStairs == false && testLocation.isStairs()) continue;

                        WalkableLocations.Add(testLocation);
                    }
                }
            }

            if (WalkableLocations.Count == 0) return false;
            else
            {
                double closestDistance = 99.0;
                Location closestLocation = new Location();
                foreach (Location loc in WalkableLocations)
                {
                    double currentDistance = loc.WalkDistanceTo(Player.Location);
                    if (currentDistance < closestDistance)
                    {
                        closestDistance = currentDistance;
                        closestLocation = loc;
                    }
                }

                WalkableLocation = closestLocation;
                return true;

            }
        }


    }
}
