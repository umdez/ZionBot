using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace OtClientBot
{
    public class Player
    {
               
        public static Creature AttackingCreature
        {
            get { return new Creature(AttackingCreaturePtr); }
        }

        public static uint AttackingCreaturePtr
        {
            get { return Memory.ReadUint(Address.Player.AttackingCreature); }
            set { Memory.WriteUint(Address.Player.AttackingCreature, value);  }
        }

        public static void AttackAndFollow()
        {
            SetFollowMode(true);

        }

        public static void SetFollowMode(bool Follow)
        {
            // A0 <Attack Mode> < Follow Mode > < Pvp Mode >


            var p = new Packet();

            p.AddByte(Enums.SendingOpCodes.ClientChangeFightModes);

            p.AddByte(Player.AttackMode);

            p.AddByte(Follow? (byte)1 : (byte)0);

            p.AddByte(Player.PvpMode);

            p.Send();
        }

        public static void AttackAndStand()
        {
            SetFollowMode(false);
        }

        public static uint FollowingCreature { get { return Memory.ReadUint(Address.Player.FollowingCreature); } }

        public static bool IsOnline { get { if (Memory.ReadByte(Address.Client.CharacterName) == 0) return false; return true; } }
        
        public static ushort X { get { return Memory.ReadUShort(Address.Player.X); } }
        public static ushort Y { get { return Memory.ReadUShort(Address.Player.Y); } }
        public static byte Z { get { return Memory.ReadByte(Address.Player.Z); } }


        public static byte FollowMode { get { return Memory.ReadByte(Address.Player.FollowMode); } }
        public static byte PvpMode { get { return Memory.ReadByte(Address.Player.PvpMode); } }
        public static byte AttackMode { get { return Memory.ReadByte(Address.Player.AttackMode); } }





        public static uint HP { get { return (uint)Memory.ReadDouble(Address.Player.HP); } }
        public static uint HPMAX { get { return (uint)Memory.ReadDouble(Address.Player.HPMAX); } }
        public static uint HPPC { get { return (uint)((Player.HP * 100) / Player.HPMAX); } }
        public static uint MP { get { return (uint)Memory.ReadDouble(Address.Player.MP); } }
        public static uint MPMAX { get { return (uint)Memory.ReadDouble(Address.Player.MPMAX); } }
        public static uint MPPC { get { return (uint)((Player.MP * 100) / Player.MPMAX); } }

        public static bool isWalking {  get { if (Memory.ReadByte(Address.Player.isWalking) != 0) return true; else return false; } }

        public static bool isAttacking { get { if (AttackingCreaturePtr != 0) return true; else return false; } }

        public static string Name { get { return Memory.ReadSdtString(Address.Player.Name); } }
        public static uint Id {  get { return Memory.ReadUint(Address.Player.Id); } }

        public static uint PlayerFlags { get { return Memory.ReadUint(Address.Player.Flags); } }


        public static bool isHungry { get { return CheckFlag(Flags.Hungry); } }
        public static bool isProtectionZone { get { return CheckFlag(Flags.ProtectionZone); } }
        public static bool isBattle { get { return CheckFlag(Flags.Battle); } }

        public enum Flags : uint
        {
            Battle = 128,
            Hungry = 1024,
            ProtectionZone = 2048,
        }



        public static Location Location { get { return new Location(X,Y,Z); } }


        public static bool CheckFlag(Flags _flag)
        {
            uint flag = (uint)_flag;
            return ((PlayerFlags & flag) == flag);
        }

        public static bool Cast(string spell)
        {
            Say(spell);
            return true;
        }


        public static void Say(string text)
        {
            Packet p = new Packet();
            p.AddByte(Enums.SendingOpCodes.ClientTalk); // SayOpCode
            p.AddByte(0x01); // Normal Message code
            p.AddUint16((ushort)text.Length); // Write lenght before string array
            p.AddString(text); // Write string
            p.SendPacket();
        }

        public static void Income(string hexString)
        {
            Packet p = new Packet(hexString);

            p.ClosePacket();

            Debugger.RecievePacket(p);
        }


        public static Action OnBattlelistError;


 


        public static void UseWith(ushort ItemId, Location loc)
        {
            // This will find item by id on backpack on serverside
            UseWith(new Item(ItemId), loc);
        }

        public static void UseWith(Item item, Location loc, ushort TargetId = 0)
        {
            Packet p = new Packet();
            p.AddByte(Enums.SendingOpCodes.ClientUseItemWith);
            p.AddBytes(item.Location.ushortRaw); // Add item position
            p.AddUint16(item.Id); // Add item ID
            p.AddByte(item.Location.Z); // StackPos
            p.AddBytes(loc.ushortRaw); // Add target location
            p.AddUint16(TargetId);
            p.AddByte((byte)0); // Placeholder for StackPos
            p.SendPacket();
        }


        public static void RemoveAllItemsFrom(Location loc)
        {
            int c = 0;
            int max = 15;
            var tile = GameMap.GetTile(loc);

            while (tile.GetTopItem() != null && c < max )
            {
                Location moveItemTo;
                loc.GetItemThrowableLocationAround(out moveItemTo);
                Player.MoveItem(tile.GetTopItem(), moveItemTo);
                Thread.Sleep(200);
                c++;
            }
        }

        public static void UseGround(Location loc, byte ContainerIndex = 0xFF)
        {
            Packet p = new Packet();
            p.AddByte(Enums.SendingOpCodes.ClientUseItem);
            p.AddBytes(loc.ushortRaw);
            var TopItem = GameMap.GetTile(loc).GetTopItem();
            if (TopItem == null) return;
            p.AddUint16(TopItem.Id); // Item on the top of the tile stack
            p.AddByte(1);   // StackPos placeholder....
            p.AddByte(ContainerIndex == 0xFF ? Iventory.EmptyIndex : ContainerIndex); // The index that will be filled if the item used is a container.
            p.SendPacket();
        }



        public static void Open(Item item, byte ContainerIndex = 0xFF)
        {
            Packet p = new Packet();
            p.AddByte(Enums.SendingOpCodes.ClientUseItem);
            p.AddBytes(item.Location.ushortRaw);
            p.AddUint16(item.Id); // Item on the top of the tile stack
            p.AddByte(1);   // StackPos placeholder....
            p.AddByte(ContainerIndex == 0xFF ? Iventory.EmptyIndex : ContainerIndex); // The index that will be filled if the item used is a container.
            p.SendPacket();
        }


        public static void UseItem(Item i)
        {
            Packet p = new Packet();

            p.AddByte(Enums.SendingOpCodes.ClientUseItem);
            p.AddLocation(i.Location); // Add Location
            p.AddUint16(i.Id); // Add Item ID
            p.AddByte(i.Location.Z); // StackPos ( the slot offset is used here )
            p.AddByte(Iventory.EmptyIndex);   // Add container index that will be filled if the used item is a container ( that will be opened when used. )
            p.SendPacket();


        }



        public static void MoveItem(Item fromItem,Location toLoc, byte quantity = 1)
        {            
            Packet p = new Packet();
            p.AddByte(Enums.SendingOpCodes.ClientMove);
            p.AddBytes(fromItem.Location.ushortRaw);
            p.AddUint16(fromItem.Id); // It looks that it's mandatory to send the exact item's id that's being moved unless you are moving a creature
            if (fromItem.isOnIventory) p.AddByte(fromItem.Location.Z); // StackPos if it's on iventory ( represents slot offset )
            else p.AddByte(0x01); // StackPos on ground , which may be 0, 1 or 2, but in fact, this does not matters.
            p.AddBytes(toLoc.ushortRaw); // Add target location
            p.AddByte(quantity); // Quantity must be exact, if you send greater than what there is it will not move the stack.
            p.SendPacket();
        }

        public static bool AttackInRange(int range)
        {
            foreach (Creature creature in BattleList.GetAllCreatures(true))
            {
                if (creature.Location.WalkDistanceTo(Player.Location) <= range)
                {
                    Player.AttackCreature(creature);
                    return true;
                }
            }
            return false;
        }

        public static bool AttackByName(string Name)
        {
            foreach (Creature creature in BattleList.GetAllCreatures(true))
            {
                if (creature.Name.ToLower() == Name.ToLower())
                {
                    Player.AttackCreature(creature);
                    return true;
                }
            }
            return false;
        }

        public static bool AttackById(int Id)
        {
            foreach (Creature creature in BattleList.GetAllCreatures(true))
            {
                if (creature.Id == Id)
                {
                    Player.AttackCreature(creature);
                    return true;
                }
            }
            return false;
        }


        public static void Stop()
        {
            Post.Esc();

            new Packet(new byte[] { (byte)Enums.SendingOpCodes.ClientCancelAttackAndFollow }).Send();
        }


        public static Packet LastAttackCreaturePacket;
        public static bool AttackCreature(Creature c)
        {
            Packet p = new Packet();

            p.AddByte(Enums.SendingOpCodes.ClientAttack);
            p.AddUint32(c.Id);
            p.Send();
            LastAttackCreaturePacket = p;
            Debugger.SetAttackingCreature(c);

            new System.Threading.Tasks.TaskFactory().StartNew(() => {
                Thread.Sleep(500);
                if (p == LastAttackCreaturePacket) p.Send();
            }); 

            


            




            return true;
        }


        public static bool WalkTo(Location loc)
        {
            
            // If Location is on screen then tries to walk to it using A*
            if(loc.isOnScreenMemory())
            {
                var path = new Utilities.PathFinder().FindPath(loc);
                if (path.Count() > 0)
                {
                    var packet = new Packet();
                    packet.AddByte(Enums.SendingOpCodes.ClientAutoWalk);
                    packet.AddByte((byte)path.Count());


                    foreach (byte dir in path)
                    {
                        byte _byte;
                        switch (dir)
                        {
                            case 1:
                                _byte = 1;
                                break;
                            case 4:
                                _byte = 2;
                                break;
                            case 0:
                                _byte = 3;
                                break;
                            case 7:
                                _byte = 4;
                                break;
                            case 3:
                                _byte = 5;
                                break;
                            case 6:
                                _byte = 6;
                                break;
                            case 2:
                                _byte = 7;
                                break;
                            case 5:
                                _byte = 8;
                                break;
                            default:
                                _byte = 0;
                                break;
                        }
                        packet.AddByte(_byte);
                    }

                    string savedLoc = Player.Location.ToString();

                    packet.Send();
                    Client.WaitPing(300);
                    //Client.WaitPing();

                    // If player did not move then return false
                    if (Player.isWalking || savedLoc != Player.Location.ToString()) return true;
                }
            }

            Debugger.ActiveWalkError();
            Debugger.WalkTo(loc);
            Client.WaitPing(200);

            if (Debugger.WalkSucceded) return true;
            else return false;

        }

        public static bool WalkTo(ushort X, ushort Y, byte Z)
        {
            return WalkTo(new Location(X, Y, Z));
        }






    }
}
