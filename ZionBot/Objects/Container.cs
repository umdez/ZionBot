using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Container
    {
        public static uint ContainerYStart = 0x80;

        public uint address;

        public int Index;
        public int Capacity;
        public Item ContainerItem;
        public string Name;
        public bool hasParent;
        public bool isClosed;

        private List<Item> _Items;

        public List<Item> Items
        {
            get
            {
                UpdateItems();
                return _Items;
            }

            set
            {
                _Items = value;
            }
        }

        public bool isValid = true;


        private uint firstIndex;
        private int dequeStart;
        private int dequeSize;


        public int ItemCount
        {
            get
            {
                return Items.Count;
            }
        }

        public bool IsFull
        {
            get
            {
                return Capacity == ItemCount;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return ItemCount == 0;
            }
        }
        
        public int EmptySlot
        {
            get
            {
                if (IsFull)
                    return -1;

                return ItemCount;
            }
        }




        public Container(uint address)
        {
            this.address = address;

            this.Index = Memory.ReadInt(address+ (uint)Address.Container.Offsets.Id);
            this.Capacity= Memory.ReadInt(address + (uint)Address.Container.Offsets.Capacity);
            this.Name = Memory.ReadSdtString(address + (uint)Address.Container.Offsets.Name);
            this.hasParent = Memory.ReadBool(address + (uint)Address.Container.Offsets.HasParent);
            this.isClosed = Memory.ReadBool(address + (uint)Address.Container.Offsets.IsClosed);
            this.firstIndex = Memory.ReadUint(address + (uint)Address.Container.Offsets.ItemsDequeFirstIndex);
            this.dequeStart = Memory.ReadInt(address + (uint)Address.Container.Offsets.ItemsDeque);
            this.dequeSize = Memory.ReadInt(address + (uint)Address.Container.Offsets.ItemsDequeSize);

            this.Items = new List<Item>();
            UpdateItems();

            if (address == 0 || String.IsNullOrWhiteSpace(Name) || Index > 1000 || firstIndex > 50)
            {
                isValid = false;
            }
        }


        public Location GetEmptySlot()
        {
            int SlotIndex = this.EmptySlot;
            if (SlotIndex != -1) // If the container is not full
            {
                return new Location(0xFFFF, (ushort)(this.Index + ContainerYStart), (byte)SlotIndex);
            }
            else return null;
        }

        public Location GetLootSlot(Item srcItem = null)
        {
            if (srcItem != null)
            {
                Item i = FindItem(srcItem.Id);

                if (i != null && (i.Count + srcItem.Count <= 100 || this.IsFull == false))
                {
                    return i.Location;
                }
            }


            return GetEmptySlot();
        }



        public void StackItem(ushort itemId)
        {
            List<Item> ItemsToStack = (from item in Items where item.Id == itemId && item.Count < 100 orderby item.Location.Z ascending select item).ToList();

            if (ItemsToStack.Count() <= 1) return;


            /**
             * 
             * Get first item
             * Calculate the ammount that it needs to fill it self
             * 
             * Take all subsequent items and move the ammount necessary to fill the first item. Start from the last to the first.
             * 
             * Call Itself Again.
             * 
             * >> This function has a fucking overhead due to the recursion and list operations, if necessary come here and [OPTIMIZE] it
             * 
             * */


            int AmountNecessaryToFill = 100 - ItemsToStack[0].Count;

            Location moveTo = ItemsToStack[0].Location;

            ItemsToStack.RemoveAt(0);
            ItemsToStack.Reverse();


            foreach (Item item in ItemsToStack)
            {
                int AmountThatWillBeMoved = Math.Min(AmountNecessaryToFill, item.Count);

                Player.MoveItem(item, moveTo, (byte)AmountThatWillBeMoved);

                Client.WaitPing(100);

                if (AmountNecessaryToFill <= 0) break;

            }


            StackItem(itemId);

            Program.Log("Stacking: " + itemId.ToString() + " at " + this.Index.ToString());

        }




        public void StackItems()
        {
            // Well, we will need a list of ids from items that are stackable.

            foreach(ushort itemId in 
                (from item in this.Items
                where (OtClientBot.Items.StackableItems.Contains(item.Id) || (from _i in this.Items where _i.Count > 1 select _i.Id).Contains(item.Id)) && OtClientBot.Items.ExceptionStackable.Contains(item.Id) == false
                select item.Id).Distinct())
            {
                StackItem(itemId);
            }
            
            //throw new NotImplementedException();
        }

        public void UpdateItems()
        {
            _Items = new List<Item>();

            this.firstIndex = Memory.ReadUint(address + (uint)Address.Container.Offsets.ItemsDequeFirstIndex);
            this.dequeStart = Memory.ReadInt(address + (uint)Address.Container.Offsets.ItemsDeque);
            this.dequeSize = Memory.ReadInt(address + (uint)Address.Container.Offsets.ItemsDequeSize);


            List<uint> itemsAddrs = Utils.IterateDeque(address, (uint)dequeStart, dequeSize, (int)firstIndex);

            byte SlotPos = 0;
                        
            foreach (uint itemAdr in itemsAddrs)
            {
                Item item = new Item(itemAdr,new Location(0xFFFF,(ushort)(ContainerYStart+this.Index), SlotPos));
                if (item != null && item.isItem)
                {
                    _Items.Add(item);
                    SlotPos++;
                }
            }
        }





        public Item GetSlot(int slotId)
        {
            if (slotId <= ItemCount)
                return Items[slotId];

            return null;
        }

        public Item FindItem(ushort itemId)
        {
            foreach (Item i in Items)
            {
                if (i.Id == itemId) return i;
            }

            return null;
        }




        public void PrintInfo()
        {
            Console.WriteLine("|------------------ " + Name + " ------------------|");
            Console.WriteLine("| index: " + Index);
            Console.WriteLine("| Y_Loc: " + (Index+ContainerYStart).ToString("x2"));
            Console.WriteLine("| hasParent: " + hasParent);
            Console.WriteLine("| isClosed: " + isClosed);
            Console.WriteLine("| Items: " + Items.Count);
            Console.WriteLine("| Capacity: " + Capacity);
            Console.WriteLine("|---------- Items --------");

            foreach (Item i in Items)
            {
                Console.WriteLine("| ID: [" + BitConverter.ToString(BitConverter.GetBytes(i.Id)).ToString().Replace("-"," ") + "]");
                Console.WriteLine("| Count: " + i.Count);                
                Console.WriteLine("| Z_Loc: " + i.Location.Z);
            }

        }





    }
}
