using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OtClientBot
{
    public class Minimap
    {
        public const uint UNORDERED_MAP_SIZE = 0x18;

        public const uint VALUE_OFFSET = 0x10;

        public const uint BLOCK_SIZE = 64;

        static uint MapStart = Address.Maps.MiniMap;

        public enum Colors : byte
        {
            Stairs = 210
        }


        public class Tile
        {
            public byte Flags;
            public byte Color;
            public byte Speed;

            public bool isWalkable()
            {
                return !((Flags & 4) == 4);
            }

            public bool isStairs()
            {
                return Color == (byte)Colors.Stairs;
            }
        }


        

        public static Tile GetTile(Location loc)
        {
            var BlockIndex = Map.getBlockIndex(loc, BLOCK_SIZE);

            var FloorMap = Map.GetFloorMap(loc.Z, MapStart);

            var TileBlock = Map.GetTileBlock(BlockIndex, FloorMap, VALUE_OFFSET);

            var TileIndex = Map.getTileIndex(loc, BLOCK_SIZE);            

            var rawTileBytes = Memory.ReadBytes(TileBlock + (TileIndex * 3), 3);

            var Tile = new Tile();

            Tile.Flags = rawTileBytes[0];
            Tile.Color = rawTileBytes[1];
            Tile.Speed = rawTileBytes[2];

            return Tile;
        }
        


        public static byte GetTileColor(Location loc)
        {
            return GetTile(loc).Color;   
        }

        public static byte GetTileColor(int x, int y)
        {
            return GetTile(new Location(x,y,Player.Z)).Color;
        }





    }
}
