using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace OtClientBot
{
    public class Client
    {
        public static Process process;


        public static IntPtr wHandle { get { return process.MainWindowHandle; } }

        public static IntPtr Base { get { return process.MainModule.BaseAddress; } }

        public static int pId { get { return process.Id; } }

        public static string LastStatusBarMessage = "";

        public static ushort LastSeenItemId = 0;



        public static uint Ping {  get { return Memory.ReadUint(Address.Client.Ping); } }
        public static string CharacterName {get { return Memory.ReadSdtString(Address.Client.CharacterName);  } }


        public static uint PacketAddress = 0;

        public static uint RecievePacketAddress = 0;

        // public static bool IsOnline {  get {if (Memory.ReadByte(Address.Client.CharacterName)==0) return false; return true;  } }

        
        public static void Wait(int n)
        {
            Thread.Sleep(n);
        }

        public static void WaitPing(int milisseconds = 0)
        {
            Math.Max(Ping, 500);
            if (milisseconds > 0 && Ping*2 < milisseconds)
            {
                Thread.Sleep(milisseconds);
            }
            else
            {
                Thread.Sleep((int)Ping*2 + 10);
            }
            
        }


        public static void NopLight()
        {
            Memory.WriteNops(Address.Client.Item_LightChange, 7);
            Memory.WriteNops(Address.Client.Cave_LightChange, 7);

            // Global light change.
        }

        
        public static void RestoreLight()
        {
            Memory.WriteBytes(Address.Client.Item_LightChange, Address.Client.op_Default_Light);
            Memory.WriteBytes(Address.Client.Cave_LightChange, Address.Client.op_Default_Light);
        }

        public static void SetLight(byte itensity, byte color = 0)
        {
            NopLight();
            var LightItensityAddress = Address.Player.PlayerStart + 0xA0;// 0XA4;
            var LightColorAddress = Address.Player.PlayerStart + 0xA0 +1;//0XA4 + 1;

            Memory.WriteByte(LightItensityAddress, itensity);
            Memory.WriteByte(LightColorAddress, color);
        }




        static Packet MessagePacket(string text, byte code = 0x17)
        {

            var p = new Packet();
            p.AddByte(0xB4);
            p.AddByte(code);
            p.AddString(text);
            p.ClosePacket();
            return p;

        }

        


        public static void Output(string str)
        {
            Console.WriteLine(str);
            Debug.WriteLine(str);
        }
        

    }
}
