using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OtClientBot
{
    public class Memory
    {

        public static IntPtr TargetHandle;



        internal static bool WriteInt(long address, int value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value), 4);
        }

        internal static bool WriteUint(long address, uint value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value), 4);
        }



        public static bool WriteUShort(long address, ushort value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value), 2);
        }

        public static bool WriteString(long address, string str)
        {
            str += '\0';
            byte[] bytes = System.Text.ASCIIEncoding.Default.GetBytes(str);
            return WriteBytes(address, bytes, (uint)bytes.Length);
        }


        internal static bool WriteByte(long address, byte value)
        {
            return WriteBytes(address, new byte[] { value }, 1);
        }

        internal static void WriteNops(long address, int nops)
        {
            byte nop = 0x90;
            int j = 0;
            for (int i = 0; i < nops; i++)
            {
                WriteBytes(address + j, new byte[] { nop }, 1);
                j++;
            }
        }




        public static byte ReadByte(long address)
        {
            return ReadBytes(address, 1)[0];
        }

        public static int ReadInt(long address)
        {
            return BitConverter.ToInt32(ReadBytes(address, 4), 0);
        }

        public static uint ReadUint(long address)
        {
            return BitConverter.ToUInt32(ReadBytes(address, 4), 0);
        }

        public static ushort ReadUShort(long address)
        {
            return BitConverter.ToUInt16(ReadBytes(address, 2), 0);
        }

        public static bool ReadBool(long address)
        {
            return BitConverter.ToBoolean(ReadBytes(address,1),0);
        }

        internal static double ReadDouble(long address)
        {
            return BitConverter.ToDouble(ReadBytes(address, 8), 0);
        }

        public static string ReadSdtString(long address)
        {
            if (ReadByte(address + 16) < 16)
                return ReadString(address, 0);
            else
                return ReadString(ReadInt(address), 0);
        }

        public static void WriteSdrString(long address, string str)
        {
            if (ReadByte(address + 16) < 16)
                WriteString(address, str);
            else
                WriteString(ReadInt(address), str);
            WriteByte(address + 16, (byte)str.Length);
        }

        public static string ReadString(long address)
        {
            return ReadString(address, 0);
        }

        public static string ReadString(long address, uint length)
        {
            if (length > 0)
            {
                byte[] buffer;
                buffer = ReadBytes(address, length);
                return System.Text.ASCIIEncoding.Default.GetString(buffer).Split(new Char())[0];
            }
            else
            {
                string s = "";
                byte temp = ReadByte(address++);
                while (temp != 0)
                {
                    s += (char)temp;
                    temp = ReadByte(address++);
                }
                return s;
            }
        }




        public static byte[] ReadBytes(long address, uint bytesToRead)
        {
            try
            {
                IntPtr ptrBytesRead;
                byte[] buffer = new byte[bytesToRead];
                ReadProcessMemory(TargetHandle, (IntPtr)address, buffer, bytesToRead, out ptrBytesRead);
                return buffer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return new byte[bytesToRead];
            }
        }



        public static bool WriteBytes(long address, byte[] bytes, uint length = 0)
        {
            if (length == 0) length = (uint)bytes.Length;
            try
            {
                IntPtr bytesWritten;
                int result = WriteProcessMemory(TargetHandle, new IntPtr(address), bytes, length, out bytesWritten);
                return result != 0;
            }
            catch { return false; }
        }









        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
    [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);


    }
}
