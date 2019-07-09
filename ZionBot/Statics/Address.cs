using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public static class Address
    {
        private static uint _Base = 0;
        public static uint Base { get { try { _Base = (uint)OtClientBot.Client.process.MainModule.BaseAddress; } catch { } ; return _Base; } }


        
        #region Player

        // ###########################################################################################################
        //                                               Player
        // ###########################################################################################################


        public static class adrPlayer
        {
            /** 
             *      Para atualizar esta sessão basta procurar pelo following creature.
             * 
             * */



            public static int PlayerPointer = 0x0546B54; //0x545A94
            public static int AttackingCreature = PlayerPointer + 4;
            public static int FollowingCreature = PlayerPointer + 8;
            public static int X = PlayerPointer + 0x4E8;
            public static int Y = X + 4;
            public static int Z = X + 8;


            public static int AttackMode = PlayerPointer - 0x9c;
            public static int FollowMode = AttackMode + 4;
            public static int PvpMode = AttackMode + 0xc;



        }



        public static class Player
        {
            public enum Offsets : uint
            {
                // Aqui basta fazer os ajustes usando o cheat engine. A estrutura de pointers facilita o trabalho.

                HP = 0x340,
                HPMAX = HP+8,
                MP = 0x378,
                MPMAX = MP+8,

                // Ficar trocando de helmet e procurando por 'Changed Value' até soubrarem poucos addresses, depois dar um pointer scan para achar um pointer de nível 1.
                EquipStart = 0x27c,//0x26C,




                Id = 0x1C,
                Name = 0x20,
                isWalking = 0x1a0, //0x188
                Flags = 0x334, //0x31C


            }





            public static uint AttackingCreature { get { return (uint)(Base + adrPlayer.PlayerPointer + 4); } }
            public static uint FollowingCreature { get { return (uint)(Base + adrPlayer.FollowingCreature); } }

            private static uint PlayerPointer { get { return (uint)(Base + adrPlayer.PlayerPointer); } }

            public static uint PlayerStart { get { return Memory.ReadUint(PlayerPointer); } }

            public static uint HP { get { return (uint)(PlayerStart+Offsets.HP); } }
            public static uint HPMAX { get { return (uint)(PlayerStart + Offsets.HPMAX); } }
            public static uint MP { get { return (uint)(PlayerStart + Offsets.MP); } }
            public static uint MPMAX { get { return (uint)(PlayerStart + Offsets.MPMAX); } }

            public static uint X { get { return (uint)(Base + adrPlayer.X); } }
            public static uint Y { get { return (uint)(Base + adrPlayer.Y); } }
            public static uint Z { get { return (uint)(Base + adrPlayer.Z); } }

            public static uint AttackMode { get { return (uint)(Base + adrPlayer.AttackMode); } }
            public static uint FollowMode { get { return (uint)(Base + adrPlayer.FollowMode); } }
            public static uint PvpMode { get { return (uint)(Base + adrPlayer.PvpMode); } }




            public static uint Id {  get { return PlayerStart + (uint)Offsets.Id; } }
            public static uint Name { get { return PlayerStart + (uint)Offsets.Name; } }
            public static uint Flags { get { return PlayerStart + (uint)Offsets.Flags; } }


            public static uint isWalking { get { return PlayerStart + (uint)Offsets.isWalking; } }

            public static uint EquipStart { get { return (uint)(PlayerStart + Offsets.EquipStart); } }

        }


        #endregion
        
        #region Hooks

        // ###########################################################################################################
        //                                               Hooks
        // ###########################################################################################################


        public enum GuiObjectOffsets : uint
        {
            X = 0x290,
            Y = 0x294,
            Width = 0x298,
            Heigth = 0x29C

        }

        public enum adrHooks : uint
        {

            WalkTo = 0x356C0, // 0x35640, //2.1 //  LocalPlayer::autoWalk(const Position& destination)

            WalkError = 0x35850, // onAutoWalkFail

            // Pesquisar por "OnOpcode"
            // Nesta função existe a chamada " msg->getU8(); ", que faz com que o packet seja movido para ECX, e é neste momento que colocaremos nosso breakpoint.
            IncomingPacket = 0xD0C6C, // 0xD0A6C,// When packet address = Ecx //2.3

            // Esta é a função SendPacket encontrada em vários locais..
            SendingPacket = 0x18BD25,  //0x18BBD5, // 2.1 // Inside "SendPacket" When packet address = Edx // 2.3

            // Pesquisar por "message too large" para encontrar a função SendTalk
            SendTalk = 0xDDF90,//0xDDD70, // Start // 2.1 // 2.3


            GetGuiObject = 0xED627,//0xED523, // This is a place where we can find the GUI Object at EDI

            MainLoop = 0x28BA9A, //0x28B78A, // This is the place on MainLoop where Medivia's client calls "PeekMessage" API function. We will call thread sensistive functions from here.



            GameObject = 0x546A80, // This + Base Address is what we will put on ECX when calling Game:: Functions

            WalkObject = 0x546A80, //0x545A94, // Object used on WalkTo function call

            ParsePacketObject = 0, //0x545AA0, 

            PacketObject = GameObject+0xE0, // 2.3

            SendPacket = 0x18BD10, //0x18BBC0, // SendPacketFunction 2.3

            RecievePacket = 0, //0xD0A00, // Recieve Packet Function


            SetAttackingCreature = 0x2B9E0, //0x2B960, // Start  2.3


            OnCreatureDeath = 0x156A0, //0x15670, // 2.3




        }

        public static class Hooks
        {
            public static IntPtr WalkTo { get { return (IntPtr)(Base + adrHooks.WalkTo); } }
            public static IntPtr WalkError { get { return (IntPtr)(Base + adrHooks.WalkError); } }
            public static IntPtr IncomingPacket { get { return (IntPtr)(Base + adrHooks.IncomingPacket); } }
            public static IntPtr SendingPacket { get { return (IntPtr)(Base + adrHooks.SendingPacket); } }
            public static IntPtr SendTalk { get { return (IntPtr)(Base + adrHooks.SendTalk); } }
            //public static IntPtr LookItem { get { return (IntPtr)(Base + adrHooks.LookItem); } }


            public static IntPtr GetGuiObject { get { return (IntPtr)(Base + adrHooks.GetGuiObject); } }

            public static IntPtr WalkObject { get { return (IntPtr)(Base + adrHooks.WalkObject); } }


            public static IntPtr MainLoop { get { return (IntPtr)(Base + adrHooks.MainLoop); } }
            public static IntPtr GameObject { get { return (IntPtr)(Base + adrHooks.GameObject); } }
            public static IntPtr PacketObject { get { return (IntPtr)(Base + adrHooks.PacketObject); } }
            public static IntPtr SendPacket { get { return (IntPtr)(Base + adrHooks.SendPacket); } }

            public static IntPtr RecievePacket { get { return (IntPtr)(Base + adrHooks.RecievePacket); } }

            public static IntPtr ParsePacketObject { get { return (IntPtr)(Base + adrHooks.ParsePacketObject); } }


            public static IntPtr SetAttackingCreature { get { return (IntPtr)(Base + adrHooks.SetAttackingCreature); } }


            public static IntPtr OnCreatureDeath { get { return (IntPtr)(Base + adrHooks.OnCreatureDeath); } }




        }
        #endregion

        #region Client

        // ###########################################################################################################
        //                                               Client
        // ###########################################################################################################

        public enum adrClient : uint
        {
            // Para encontrar estes é necessário achar os códigos que modificam a luz do player. Basta usar o "What writes to this address" no CE.
            Cave_LightChange = 0xD8A1C, // 2.3
            Item_LightChange = 0xD39D2, // 2.3


            // Uma função membro do g_map é chamada logo antes do código que altera a luz do personagem, logo encontramos uma referência ao address de g_map em algum 'mov ecx' lá perto.
            g_Map = 0x546D90, //0x545cd0, 2.3

            //  g_map + 0x264 = 0x545F34  ( Dentro da função GetCreatureById , no debugger, encontramos referência a esse endereço como sendo o de "known creatures "
                // 2.3 , bom, parece que esse 0x264 ainda está preciso. O known creatures é um 'std::unordmap', um jeito de conferir é ver se o número de known_size vai aumentando conforme mais criaturas aparecem na tela.
                
            KnownCreatures = g_Map + 0x264, // 2.3


            // Basta pesquisar pelo nome no CE e pegar o address estático
            CharacterName = 0x546AD4, // 0x5456F4, // 2.3


            // O ping fica um pouco a cima do address do Character name.
            Ping = 0x546A88, // 2.3


            // Procurar pelos valores do tamanho da janela no CE ( Increased e Decreased values, até sobrar poucos )
            // Daí escolhe um desses valores e coloca um "What Write to this Address" e modifica
            // Pega-se o valor do registrador que é somado ao offset 0x30/0x34 e pesquisa o valor dele no CE
            // Daí alguns addresses irão conter o valor desse registrador. Pega-se esses addresses e pesquisa-se novamente no CE
            // É verificado que dessa forma encontraremos uma Pointer chain de dois níveis que irá apontar para a estrutura que desejamos. 
            
            GameGuiBase = 0//0x545DA8,//  0x0545A88, // 2.1  // Por enquanto vou deixar em 0 para ver se o GuiObject é suficiente.





        }

        public static class Client
        {

            // Essas coisas devem ser obtidas de forma dinânica para maior estabilidade.
            public static string recievePacketHeader = "BC 81 89 00 00 00 00 00 FF FF FF FF 06 00 08 00 03 00 00 00 00 00 00 00 01 00 AA 00 00 00 00 00";

            public static string packetHeader = "74 92 89 00 01 00 00 00 FF FF FF FF 08 00 09 00  01 00 AD BA 0D F0 AD BA 0D F0 1D BA 0D F0 AD BA";


            public static uint Ping { get { return (uint)(Base + adrClient.Ping); } }
            public static uint CharacterName { get { return (uint)(Base + adrClient.CharacterName); } }
            public static uint ServerName { get { return (uint)(Base + adrClient.CharacterName+0x18); } }

            public static uint GameGuiBase { get { return (uint)(Base + adrClient.GameGuiBase); } }

            //public static uint isOnline { get { return (uint)(Base + adrPlayer.IsOnline); } }



            public static uint KnownCreatures { get { return (uint)(Base + adrClient.KnownCreatures); } }

            //public static uint GameEcx { get { return (uint)(Base + adrClient.GameEcx); } }


            public static uint g_Map { get { return (uint)(Base + adrClient.g_Map); } }
            public static uint Cave_LightChange { get { return (uint)(Base + adrClient.Cave_LightChange); } }
            public static uint Item_LightChange { get { return (uint)(Base + adrClient.Item_LightChange); } }

            public static byte[] op_Default_Light = new byte[] { 0x66, 0x89, 0x81, 0xB4, 0x00, 0x00, 0x00 }; // mov [ecx+000000B4],ax
        }

        #endregion
        
        #region Iventory


        // ###########################################################################################################
        //                                               Container
        // ###########################################################################################################


        public enum adrContainer : uint
        {
            // A lista de containers é um "custom" sdt_unordmap onde as é o "id" do container ( 0x41 , 0x42.. )

            // Existem referências para esse addres estático no código do medivia. Basta pesqusiar usando o código fonte e um assembly.

            // Se tratando de um sdt_unordmap , podemos achar ele na memória aumentando o número de containers 'conhecidos' um por um e procurando pelos tamanho do sdt_map no CE.
                          
            // Ok, estamos na versão 2.3 e encontrei uma estrutura estática que aparentemente se trata de um std_unordmap, agora tenho que descobrir qual address colocar aqui.
                // Analizando o código fonte eu vi que o 'known size' está no offset 0x8, então fui lá e copiei o address de um 0x00000 8 bytes antes do known size
            ContainerList = 0x546B64  // 2.3



        }

public static class Container
{
         public enum Offsets : uint
         {
                 Id = 0x0C,
                 Capacity = 0x10,
                 Item = 0x14,
                 Name = 0x18,
                 HasParent = 0x30,
                 IsClosed = 0x31,
                 ItemsDeque = 0x40,
                 ItemsDequeFirstIndex = 0x48,
                 ItemsDequeSize = 0x4c,
         }



     public static uint EquipStart { get { return Player.EquipStart; } }
     public static uint ContainerList { get { return (uint)(Base + adrContainer.ContainerList); } }


}

#endregion


#region BattleList


// ###########################################################################################################
//                                               Battlelist
// ###########################################################################################################




    public static class BattleList
    {

        public static uint KnownCreatures { get { return Address.Client.KnownCreatures;  } }


    }


        public enum ThingType : byte
        {
            Item = 0x34,
            Creature = 0x2C,
            Self = 0x98,
            Player = 0xE0,
            Npc = 0xC8
        }









        #endregion






        #region Maps


        // ###########################################################################################################
        //                                               Maps
        // ###########################################################################################################

        public enum adrMaps : uint
        {
            // Esses são Arrays de sdt_unordered_maps..

            // Aparentemente todo sdt_unord_map contém um 3F80000 no meio dos seus bytes, ele fica no offset 0xC

            // Então para encontrarmos o Start, procuremos pelo primeiro 3F8 no array de sdt_maps e subtraimos 0xC

            // o GameMap é o primeiro, ele fica logo em cima do Known_Creatures.

            // o miniMap deve estar em baixo, basta scrollar até achar outro array desse tipo

                GameMap = 0x546E74, // 2.3
                

                MiniMap = GameMap + 0x29C // 0x546050 //2.3


        }


        public static class Maps
        {

                 public static uint MiniMap { get { return Base + (uint)adrMaps.MiniMap; } }
                 public static uint GameMap { get { return Base + (uint)adrMaps.GameMap; } }

        }




#endregion


}
}
