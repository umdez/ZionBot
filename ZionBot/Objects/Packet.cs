using System;
using System.Text;

namespace OtClientBot
{
    public class Packet
    {
        public ushort Lenght { get; private set; }
        public byte[] Buffer { get; private set; }
        public byte[] Raw { get; private set; }

        public byte[] buildBuffer;
        private short writePos;

        bool isBuild = false;
        bool isClosed = true;

        public Packet() {
            isBuild = true;
            isClosed = false;
            this.buildBuffer = new byte[6535];
            this.writePos = 0;
        }

        
        public void SendPacket()
        {
            Send();
        }

        public void Send()
        {
            if (isBuild)
            {
                if (!isClosed)
                {
                   this.ClosePacket();
                }
            }
            Debugger.SendPacket(this);
        }



        public void AddLocation(Location loc)
        {
            AddBytes(loc.ushortRaw);
            writePos += 5;
        }



        public void AddUint32(uint num)
        {
            BitConverter.GetBytes(num).CopyTo(buildBuffer, writePos);
            writePos += 4;
        }

        public void AddUint16(ushort num)
        {
            BitConverter.GetBytes(num).CopyTo(buildBuffer, writePos);
            writePos += 2;
        }

        public void AddByte(Enums.SendingOpCodes OpCode)
        {
            AddByte((byte)OpCode);
        }

        public void AddByte(byte num)//, bool @byte = true)
        {
            BitConverter.GetBytes(num).CopyTo(buildBuffer, writePos);
            writePos += 1;
        }

        public void AddString(string str)
        {
            byte[] strData = ASCIIEncoding.ASCII.GetBytes(str);
            strData.CopyTo(buildBuffer, writePos);
            writePos += (short)strData.Length;
        }

        public uint GetRemoteAddress()
        {
            WritePacket(Client.PacketAddress);

            return Client.PacketAddress;
        }

        public void AddLenWText(string str)
        {
            AddUint16((ushort)str.Length);
            AddString(str);
        }


        public void AddBytes (byte[] bytes)
        {
            bytes.CopyTo(buildBuffer, writePos);
            writePos += (short)bytes.Length;
        }

        public void ClosePacket()
        {
            Buffer = GetRaw(buildBuffer,writePos);

            Lenght = (ushort)writePos;

            Raw = new byte[Lenght + 0xA];

            BitConverter.GetBytes(Lenght).CopyTo(Raw, 0);

            Buffer.CopyTo(Raw, 0xA);


        }


        public byte[] GetRaw(byte[] Buffer,int size)
        {
            byte[] Raw = new byte[size];


            for (int i = 0; i<size; i++)
            {
                Raw[i] = Buffer[i];
            }

            return Raw;
        }



        public Packet(string hex)
        {
            var data = Utils.StringToByteArrayFastest(hex);

            if (data.Length > 65535)
            {
                throw new Exception("Sorry, buffer is too large to be sent");
            }

            Buffer = data;

            Lenght = (ushort)data.Length;

            Raw = new byte[Lenght + 0xA];

            BitConverter.GetBytes(Lenght).CopyTo(Raw, 0);

            Buffer.CopyTo(Raw, 0xA);
        }

        public Packet(byte[] data)
        {
            if (data.Length > 65535)
            {
                throw new Exception("Sorry, buffer is too large to be sent");
            }

            Buffer = data;

            Lenght = (ushort)data.Length;

            Raw = new byte[Lenght + 0xA];

            BitConverter.GetBytes(Lenght).CopyTo(Raw, 0);

            Buffer.CopyTo(Raw, 0xA);
        }

        public void WritePacket(long address)
        {
            Memory.WriteBytes(address + 0x10, Raw);
        }

        public void WriteIncomingPacket(long address)
        {
            /*

            004D0A4E | 66 | nop |
            004D0A50 | 8B | mov ebx,dword ptr ds:[esi] |
            004D0A52 | 0F | movzx eax,word ptr ds:[ebx + C] |
            004D0A56 | 0F | movzx ecx,word ptr ds:[ebx + E] |
            004D0A5A | 2B | sub ecx,eax |
            004D0A5C | 0F | movzx eax,word ptr ds:[ebx + 10] |

           */

            Memory.WriteUint(address + 0xC,0x00080006); // 06 00 08 00

            Memory.WriteBytes(address + 0x1A, Buffer);
            Memory.WriteByte(address + 0x1A - 2, (byte)Buffer.Length);
            Memory.WriteByte(address + 0x1A - 2 - 8, (byte)(Buffer.Length + 2));

        }







    }
}