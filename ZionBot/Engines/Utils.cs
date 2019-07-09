using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Utils
    {
        private static List<uint> mapNodesAddrs = new List<uint>();
        private static uint rootPtr;
        private static uint left = 0x0;
        private static uint parent = 0x4;
        private static uint right = 0x8;
        private static int tries = 0;
        private static int maxTries = 32;

        private static List<uint> mapValuesAddrs = new List<uint>();
        private static uint Map_Value_Offset = 0x14;

        public static  List<uint> IterateMap(uint _rootPtr) // Obtain a list with values from std::Map
        {
            tries = 0;
            mapValuesAddrs.Clear();
            rootPtr = _rootPtr;
            transverseMap(rootPtr + parent);
            return mapValuesAddrs;

        }

        private static Random Rand = new Random();
        public static byte RandByte()
        {
            return (byte)Rand.Next(256);
        }

        private static void transverseMap(uint nodeAddress)
        {
            if (nodeAddress == rootPtr || nodeAddress <= 0 || nodeAddress > 0x9999999 || tries >= maxTries)
                return;

            transverseMap(Memory.ReadUint(nodeAddress + left));
            tries++;
            mapValuesAddrs.Add(Memory.ReadUint(nodeAddress + Map_Value_Offset));
            transverseMap(Memory.ReadUint(nodeAddress + right));

        }






        static uint[] IterateUnorderedMap(uint UnorderedMapAdress)
        {
            // First we define variables with the offset values for sake of readbility.         
            uint UnorderedMap_Count_Offset = 0x8;
            uint UnorderedMap_BufferPointer_Offset = 0x4;
            uint Buffer_NodePointer_Offset = 0x0;
            uint Node_NextNode_Offset = 0x4;
            uint Node_Value_Offset = 0xC;

            // Read the memory address where the number of elements inside the UnorderedMap is Stored.
            int ArrayCount = Memory.ReadInt(UnorderedMapAdress + UnorderedMap_Count_Offset);

            // Alocate memory for a new uint array of size "ArrayCount"
            uint[] ResultArray = new uint[ArrayCount];

            // If Array is Empty then Return
            if (ArrayCount == 0) return ResultArray;



            // We Read the buffer address from the UnorderedMap object at offset 0x4, which is a pointer to the Buffer Address space
            uint BufferAdress = (uint)Memory.ReadInt(UnorderedMapAdress + UnorderedMap_BufferPointer_Offset);
            // Since Memory.ReadInt returns an Integer we need to cast a (uint) to convert it into an Uint


            // The first offset inside the Buffer Address space is a pointer to the first node
            uint CurrentNodePointer = BufferAdress + Buffer_NodePointer_Offset;


            int c = 0; // A loop variable
            while (true)
            {
                // Reads the Node address from the Current Node Pointer variable.
                uint NodeAdressSpace = (uint)Memory.ReadInt(CurrentNodePointer);

                // Read the pointer to value at the offset 0x3C inside the Node Address space.
                uint PointerToObject = (uint)Memory.ReadInt(NodeAdressSpace + Node_Value_Offset);


                // Stores the pointer address inside the array at position "c"
                ResultArray[c] = PointerToObject;

                // Increment the loop variable.             
                c++;

                if (c < ArrayCount)
                {
                    // This will store the pointer adress of the next node of the unordered map at the "CurrentNodePointer" variable.
                    CurrentNodePointer = NodeAdressSpace + Node_NextNode_Offset;
                }
                else break;
            }

            // Returns the array filled with the address to the objects that were stored inside the unordered map.
            return ResultArray;
        }




        public static List<uint> IterateDeque(uint parent, uint dequeStart, int dequeSize, int firstIndex) // Obtain a list with values from std::Deque
        {

            List<uint> childs = new List<uint>();
            for (int i = 0; i <= 0x1c; i += 4) //readind the dynamic size array, 0x1c elements
            {
                for (int j = 0; j <= 0x0c; j += 4) //reading the fixed size array of 4 elements 0x0, 0x4, 0x8 and 0xC
                {
                    //read deque position (i, j)
                    uint childrenAddr = Memory.ReadUint(dequeStart + i);
                    childrenAddr = Memory.ReadUint(childrenAddr + j);
                    childs.Add(childrenAddr);
                }
            }

            List<uint> validChilds = getOnlyValidAddresses(childs, dequeSize, firstIndex);
            return validChilds;

        }

        private static List<uint> getOnlyValidAddresses(List<uint> childs, int dequeSize, int firstIndex)
        {
            List<uint> validChilds = new List<uint>();
            int itemsRead = 0;
            int maxTries = 32;
            for (int i = 0; (i < childs.Count) && (itemsRead < dequeSize) && (i < maxTries); i++)
            {
                uint adr = childs[(firstIndex + i) % childs.Count];
                validChilds.Add(adr);
                itemsRead++;
            }

            return validChilds;
        }

  
        public delegate bool Condition(uint value);

        public static Dictionary<uint, uint> IterateNewUnordMap(uint UnordMapAddress, Condition CheckIfIsValid)
        {

            /* 
               A função não estava retornando todos os valores pois o contador do iterador estava em bytes e o buffer size
               estava mostrando o tamanho do buffer em DWORDs e não em BYTEs
               
                Agora que o buffer size foi multiplicado por 4 o iterador está funcionando corretamente. 


             */

            int KnownSize = Memory.ReadInt(UnordMapAddress + 0x8);

            int BufferSize = Memory.ReadInt(UnordMapAddress + 0x4) * 4;

            uint BufferAddress = Memory.ReadUint(UnordMapAddress + 0x14);


            Dictionary<uint, uint> KeyPairDictionary = new Dictionary<uint, uint>();

            uint CurrNodeAddress = 0;
            uint Key = 0;
            uint Value = 0;

            for (int x = 0; x < BufferSize; x += 4)
            {
                if (KeyPairDictionary.Count == KnownSize)
                {
                    Console.WriteLine(string.Format("Already iterated over all. [{0}/{1}]",x,BufferSize) );
                    break; // Break when all results have been found

                }
                if (x==8*4)
                {
                    var a = 1 + 1;


                }
                CurrNodeAddress = Memory.ReadUint(BufferAddress + x);

                if (CurrNodeAddress == 0) continue;

                do
                {

                    Key = Memory.ReadUint(CurrNodeAddress + 0x4);
                    Value = Memory.ReadUint(CurrNodeAddress + 0xC);


                    if (KeyPairDictionary.ContainsKey(Key) == false && CheckIfIsValid(Value) )
                    {
                        KeyPairDictionary.Add(Key, Value);
                    }

                    CurrNodeAddress = Memory.ReadUint(CurrNodeAddress);


                } while (CurrNodeAddress != 0);

            }


            return KeyPairDictionary;
        }





        public static uint GetValueFromNode(uint UnordMapAddress, uint Key) // This is responsible for finding the start address of TileMap Array.
        {
            var BufferAddress = Memory.ReadUint(UnordMapAddress + 0x14);

            var BufferSize = Memory.ReadUint(UnordMapAddress + 0x4);

            if (BufferSize == 0) goto Finalize;

            var ArrayOffset = 4 * (Key % BufferSize);


            var NodeAddress = Memory.ReadUint(BufferAddress + ArrayOffset);

            do
            {

                var NodeKey = Memory.ReadUint(NodeAddress + 0x4);

                if (NodeKey == Key) return Memory.ReadUint(NodeAddress + 0xC); 

                NodeAddress = Memory.ReadUint(NodeAddress);

            } while (NodeAddress != 0);


            Finalize:
            //Program.Log("Error reading Minimap: Could not get TileMap Node");
            return 0;
        }




        public static Rectangle findBitmap(Bitmap smallBmp, Bitmap bigBmp, double tolerance)
        {
            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);

            return location;
        }






        public static byte[] StringToByteArrayFastest(string hex)
        {
            hex = hex.Replace(" ", "").Replace("\n", "").Replace("\r", "");
            if (hex.Length % 2 == 1)
                hex = hex.Remove(hex.Length - 1);

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < (hex.Length >> 1); ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }


    }
}
