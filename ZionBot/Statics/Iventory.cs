using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{

    /*  Anotations:
     *  
     *  
     *      Needs to:
     *      - Add item search on equipaments
     *      - Add empty slot search on Hands and arrow slot.
     *        
     *  
     *  */






    public class Iventory
    {
        // Gets the container index that will be filled if the used item is a container ( that will be opened when used. )
        // This is used on the UseItem context.
        public static byte EmptyIndex { get { return GetEmptyIndex(); } } 
        

        

        public enum EquipSlot : ushort
        {
            Head = 0x1,
            Necklace = 0x2,
            BackPack = 0x3,
            Armor = 0x4,
            RightHand = 0x5,
            LeftHand = 0x6,
            Legs = 0x7,
            Boots = 0x8,
            Ring = 0x9,
            Arrow = 0xA,
        }

        public static byte ContainerY = 0x80; // Y value of the first opened container


        public static ushort IventoryX = 0xFFFF;




        public static Item GetSlot(EquipSlot slot)
        {            
                
                
                uint EquipStart = Address.Container.EquipStart;
                uint SlotOffset = (uint)(slot - 1) * 4;
                uint SlotAddress = EquipStart + SlotOffset;

                uint itemAddres = Memory.ReadUint(SlotAddress);
                Location itemLocation = new Location(IventoryX, (ushort)slot, 0);

                var Item = new Item(itemAddres, itemLocation);
                return Item;
        }


        public static Location SlotLocation(EquipSlot slot)
        {
            return new Location(IventoryX, (ushort)slot, 0);
        }



        static bool CheckIfContainerIsValid(uint ContainerPtr)
        {
            if (new Container(ContainerPtr).isClosed==false) return true;
            return false;
        }
        
        public static List<Container> GetAllContainers()
        {
            var Cond = new Utils.Condition(CheckIfContainerIsValid);
            var KeyPairDictionary = Utils.IterateNewUnordMap(Address.Container.ContainerList,Cond);
            List<Container> containers = new List<Container>();
            
            foreach (uint containerAdr in KeyPairDictionary.Values)
            {                
                Container container = new Container(containerAdr);

                if (container.isValid==false)
                    continue;
                containers.Add(container);
            }

            return containers;
        }

        public static Container GetContainerByIndex(int index)
        {
            foreach (Container container in GetAllContainers())
            {
                if (container.Index == index)
                    return container;
            }

            Console.WriteLine("CotainerReader: Could not found a container with index: " + index);
            return null;
        }


                

        public static Location FindEmptySlot(bool OnlyOnContainers = false, List<byte> Except = null )
        {
            // Look for empty slot on opened containers
            foreach (Container container in GetAllContainers())
            {
                if (Except != null && Except.Contains((byte)container.Index))
                    continue;


                int SlotIndex = container.EmptySlot;
                if(SlotIndex!=-1) // If the container is not full
                {
                    return new Location(0xFFFF, (ushort)(container.Index + ContainerY), (byte)SlotIndex);
                }

            }

            if (OnlyOnContainers) return null;

            // Look for empty slot on Right Hand, Left Hand and Arrow Slot.

            if (!GetSlot(EquipSlot.RightHand).isItem) return new Location(IventoryX, (ushort)EquipSlot.RightHand, 0);
            else if (!GetSlot(EquipSlot.LeftHand).isItem) return new Location(IventoryX, (ushort)EquipSlot.LeftHand, 0);
            else if (!GetSlot(EquipSlot.Arrow).isItem) return new Location(IventoryX, (ushort)EquipSlot.Arrow, 0);
            Program.Log("Could not find empty slot on iventory.");

            return null;
        }

        public static Location FindLootSlot(Item srcItem , List<byte> Except = null)
        {
            // Look for item slot on opened containers
            foreach (Container container in GetAllContainers())
            {

                // Se for a excessão, continua
                if (Except != null && Except.Contains((byte)container.Index))
                    continue;

                // Procura pelo item destino
                Item destItem = FindItem(srcItem.Id,Except);


                // Se o return for nulo então continua para o próximo container.
                if (destItem == null) continue;


                // Se o container não estiver full ele move sem problemas, se estiver então ele considera se a soma vai dar overflow na pilha de items.
                if (container.IsFull == false || destItem.Count + srcItem.Count <= 100 )
                {
                    return destItem.Location;

                }

            }

            return FindEmptySlot(Except:Except);

        }

        public static Item FindItem(ushort itemId,List<byte> Except = null)
        {
            

            Item onEquip = FindEquip(itemId);
            if (onEquip != null) return onEquip;

            foreach (Container container in GetAllContainers())
            {
                if (Except != null && Except.Contains((byte)container.Index))
                    continue;

                foreach (Item item in container.Items)
                {
                    if (item.Id == itemId) return item;
                }
            }
            Console.WriteLine("No item with id: " + itemId + " was found on any container.");
            return null;
        }


        public static byte GetEmptyIndex()
        {
            byte index = 0;

            while (index < 32)
            {
                var value = Utils.GetValueFromNode(Address.Container.ContainerList, index);

                if (value == 0) break; // Iterrompe o loop e retorna.

                index++;
            }


            return index;
        }

        public static void CloseContainer(byte index)
        {
            new Packet(new byte[] { (byte)Enums.SendingOpCodes.ClientCloseContainer , index }).Send();
        }

        public static void OpenAllContainers()
        {
            throw new NotImplementedException();
        }

        public static Item FindEquip(ushort itemId)
        {

            for (int slot = 1; slot <= 11; slot++)
            {
                uint EquipStart = Address.Container.EquipStart;
                uint SlotOffset = (uint)(slot - 1) * 4;
                uint SlotAddress = EquipStart + SlotOffset;

                uint itemAddres = Memory.ReadUint(SlotAddress);
                Location itemLocation = new Location(IventoryX, slot, 0);

                var Item = new Item(itemAddres, itemLocation);

                if (Item.Id == itemId)
                    return Item;

            }
            return null;
        }


        public static void PrintAllEquips()
        {

            for (int slot = 1; slot <= 11; slot++)
            {

                uint EquipStart = Address.Container.EquipStart;
                uint SlotOffset = (uint)(slot - 1) * 4;
                uint SlotAddress = EquipStart + SlotOffset;

                uint itemAddres = Memory.ReadUint(SlotAddress);
                Location itemLocation = new Location(IventoryX, slot, 0);

                var Item = new Item(itemAddres, itemLocation);
                Item.Print();
            }


        }


        public static void PrintAllContainers()
        {
            foreach(Container c in GetAllContainers())
            {
                c.PrintInfo();
            }
        }

        public static void StackItems()
        {
            foreach (Container c in GetAllContainers())
            {
                c.StackItems();
            }
        }
    }
}
