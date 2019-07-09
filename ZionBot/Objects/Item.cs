using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Item
    {
        public ushort Id { get; internal set; }
        public Location Location { get; internal set; }

        public byte Count { get; internal set; }

        public bool isOnIventory { get { return Location.isIventory; } }

        public bool isItem = true;


        enum Offsets : uint
        {
            Id = 0x1A,
            Count = 0x1E,

        }

        public Item(uint Address, Location loc, bool fromAddress=true)
        {
            if (Address == 0)
            {
                isItem = false;
                return;
            }

            this.Id = Memory.ReadUShort(Address + (uint)Offsets.Id);

            if(this.Id==0)
            {
                isItem = false;
                return;
            }

            this.Count = Memory.ReadByte(Address + (uint)Offsets.Count);
            this.Location = loc;            
            
        }

        public Item(ushort id,Location loc)
        {
            this.Id = id;
            this.Location = loc;
        }

        public Item(ushort id)
        {
            this.Id = id;
            this.Location = new Location(0xFFFF, 0, 0);
        }


        public override string ToString()
        {
            return string.Format("isValid: {3} Id: {0} , Count: {1} , Location: {2}", Id.ToString(), Count.ToString(), Location.ToString(), isItem.ToString());
        }

        public void Print()
        {
            if(isItem)
            Program.Log(this.ToString());

        }


    }
}