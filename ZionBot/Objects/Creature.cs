using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Creature
    {

    
        enum CreatureOffsets : uint
        {
            Type = 0,
            Light = 0XA4,
            Name = 0x20,
            X = 0xC,
            Y = 0x10,
            Z = 0x14,
            Direction = 0x3C,
            Id = 0x1C,
            Hppc= 0x38,
            baseSpeed = 0xA8,

        }




        public string Name;

        public uint Id;

        public Location Location
        {
            get { return new Location((int)X, (int)Y, (int)Z); }
        }

        public uint X { get { return Memory.ReadUShort(CreaturePtr + (uint)CreatureOffsets.X); } }
        public uint Y { get { return Memory.ReadUShort(CreaturePtr + (uint)CreatureOffsets.Y); } }
        public uint Z { get { return Memory.ReadByte(CreaturePtr + (uint)CreatureOffsets.Z); } }


        public int Hppc { get { return Memory.ReadByte(CreaturePtr + (uint)CreatureOffsets.Hppc); } }

        public int Type;

        public uint CreaturePtr;

        



        public Creature(uint CreaturePtr)
        {
            this.CreaturePtr = CreaturePtr;

            Name = Memory.ReadSdtString(CreaturePtr + (uint)CreatureOffsets.Name);
            Id = Memory.ReadUint(CreaturePtr + (uint)CreatureOffsets.Id);     

        }

        public bool isPlayer()
        {
            if (Memory.ReadByte(CreaturePtr) == (byte)Address.ThingType.Player) return true;
            return false;
        }

        public bool isNpc()
        {
            if (Memory.ReadByte(CreaturePtr) == (byte)Address.ThingType.Npc) return true;
            return false;
        }

        public bool isCreature()
        {
            if (Memory.ReadByte(CreaturePtr) == (byte)Address.ThingType.Creature) return true;
            return false;
        }

        public bool isSelf()
        {
            if (Memory.ReadByte(CreaturePtr) == (byte)Address.ThingType.Self) return true;
            return false;
        }

        public void Print()
        {
            Program.Log(this.ToString());
        }

        public override string ToString()
        {
            string PreFix = "";

            if (isPlayer()) PreFix = "Player: ";
            else if (isCreature()) PreFix = "Creature: ";
            else if (isNpc()) PreFix = "Npc: ";
            else PreFix = "Unknown: " + Memory.ReadByte(CreaturePtr).ToString("x2") + " ";

            return (PreFix + Name + " " + Location.ToString() + " 0x" + CreaturePtr.ToString("X8"));
        }





    }
}
