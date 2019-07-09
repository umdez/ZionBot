using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Map
    {
        // Essa classe serve para reunir os métodos que existem em comum entre o GameMap e o Minimap.
                

        public const uint UNORDERED_MAP_SIZE = 0x18;


        public static uint GetTileBlock(uint blockIndex, uint floorMap, uint ValueOffset) // This is responsible for finding the start address of TileMap Array.
        {
            var BufferAddress = Memory.ReadUint(floorMap + 0x14);

            var BufferSize = Memory.ReadUint(floorMap + 0x4);

            if (BufferSize == 0) goto Finalize;

            var ArrayOffset = 4 * (blockIndex % BufferSize);


            var NodeAddress = Memory.ReadUint(BufferAddress + ArrayOffset);

            do
            {

                var NodeKey = Memory.ReadUint(NodeAddress + 0x4);

                if (NodeKey == blockIndex) return NodeAddress + ValueOffset; // 0xC for Map and 0x10 for MiniMap

                NodeAddress = Memory.ReadUint(NodeAddress);

            } while (NodeAddress != 0);


            Finalize:
            Program.Log("Could not get TileBlock");
            return 0;
        }




        public static uint GetFloorMap(uint Z,uint MapStart)
        {
            var Offset = Z * UNORDERED_MAP_SIZE;

            return MapStart + Offset;
        }


        public static uint getTileIndex(Location loc, uint BLOCK_SIZE)
        {
            return getTileIndex(loc.X, loc.Y,BLOCK_SIZE);
        }

        public static uint getBlockIndex(Location loc, uint BLOCK_SIZE)
        {
            return getBlockIndex(loc.X, loc.Y,BLOCK_SIZE);
        }
        public static uint getTileIndex(int X, int Y, uint BLOCK_SIZE)
        { return (uint)(((Y % BLOCK_SIZE) * BLOCK_SIZE) + (X % BLOCK_SIZE)); }


        public static uint getBlockIndex(int X, int Y, uint BLOCK_SIZE)
        { return ((uint)(Y / BLOCK_SIZE) * (65536 / BLOCK_SIZE)) + (uint)(X / BLOCK_SIZE); }









    }
}
