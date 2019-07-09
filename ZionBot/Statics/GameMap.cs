using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class GameMap
    {
        
        public const uint VALUE_OFFSET = 0xC;

        public const uint BLOCK_SIZE = 32;

        static uint MapStart = Address.Maps.GameMap ;


        public class Tile
        {
            // Procurar pelo speed do tile na tela depois
            public enum TileOffsets : uint
            {
                ThingsQuantity = 0x16,
                ThingsPtr = 0x17,
                DrawElevation = 0x28,
                X= 0x1C,
                Y= 0x1C + 4,
                Z= 0x1C + 8


            }



            public uint Id
            {
                get
                {
                    return Memory.ReadUShort(Memory.ReadUint(ThingsPtr)+0x1A); // ( The (Thing*)TilePtr is the first DWORD on ThingsPtr ) ( And (Item)Thing ID is at offset 0x1A  
                }
            }


            public uint ThingsPtr
            {
                get
                {
                    return Memory.ReadUint(TilePtr + (uint)TileOffsets.ThingsPtr);
                }

            }

            public byte ThingsCount
            {
                get
                {
                    return Memory.ReadByte(TilePtr + (uint)TileOffsets.ThingsQuantity);
                }

            }
            public byte DrawElevation
            {
                get
                {
                    return Memory.ReadByte(TilePtr + (uint)TileOffsets.DrawElevation);
                }

            }

            public Location Location { get { return new Location(X, Y, Z); } }               

            


            public int X { get { return Memory.ReadUShort(TilePtr + (uint)TileOffsets.X); } }

            public int Y { get { return Memory.ReadUShort(TilePtr + (uint)TileOffsets.Y); } }

            public byte Z {  get { return Memory.ReadByte(TilePtr + (uint)TileOffsets.Z);   }  }


            uint TilePtr;
           

            public List<Item> GetItems()
            {
                List<Item> Items = new List<Item>();
                int count = ThingsCount;

                for (int offset = 0x4; offset < (0x4 * count); offset += 4)
                {
                    uint ThingPtr = Memory.ReadUint(ThingsPtr + offset);

                    if (isThingItem(ThingPtr) == false) continue;

                    var Item = new Item(ThingPtr, Location);

                    Items.Add(Item);
                }

                return Items;
            }

            public Item GetTopItem()
            {
                if (isTopItem() == false && ThingsCount <=2) return null;

                int count = ThingsCount;

                // Loop para encontrar o primeiro item dentro do array. , se for uma creature então pula.
                for (int offset = 0x4; offset < (0x4 * count); offset += 4)
                {
                    uint ThingPtr = Memory.ReadUint(ThingsPtr + offset);

                    if (isThingItem(ThingPtr) == false) continue;

                    var Item = new Item(ThingPtr, Location);

                    return Item;
                }

                return null;
            }

            public bool isThingItem(uint ThingPtr)
            {
                var Type = Memory.ReadByte(ThingPtr);

                //Program.Log("Thing type is: " + Type.ToString("x2").ToUpper());

                if (Type == (byte)Address.ThingType.Item) return true;

                return false;
            }


            public bool isTopItem()
            {
                if (ThingsCount <= 1) return false;

                var TopThingPtr = Memory.ReadUint(ThingsPtr + 0x4); // Offset 0x4 is Top thing.

                return isThingItem(TopThingPtr);
            }


            public bool isThingCreature(uint ThingPtr)
            {
                var Type = Memory.ReadByte(ThingPtr);

                if (Type == (byte)Address.ThingType.Creature ||
                    Type == (byte)Address.ThingType.Npc ||
                    Type == (byte)Address.ThingType.Self ||
                    Type == (byte)Address.ThingType.Player)
                    return true;

                return false;

            }

            public bool isTopCreature()
            {
                if (ThingsCount <= 1) return false;

                var TopThingPtr = Memory.ReadUint(ThingsPtr + 0x4); // Offset 0x4 is Top thing.

                var Type = Memory.ReadByte(TopThingPtr);

                return isThingCreature(TopThingPtr);
            }



            public Tile(uint TilePtr)
            {
                this.TilePtr = TilePtr;               
            }
        }



        public static Tile GetTile(Location loc)
        {
            var TilePtr = GetTilePtr(loc);

            var Tile = new Tile(TilePtr);

            return Tile;
        }


        public static uint GetTilePtr(Location loc)
        {
            var BlockIndex = Map.getBlockIndex(loc, BLOCK_SIZE);

            var FloorMap = Map.GetFloorMap(loc.Z, MapStart);

            var TileBlock = Map.GetTileBlock(BlockIndex, FloorMap,VALUE_OFFSET);

            var TileIndex = Map.getTileIndex(loc, BLOCK_SIZE);

            var TilePtr = Memory.ReadUint(TileBlock + (TileIndex * 4));

            return TilePtr;
        }







    }
}
