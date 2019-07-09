using OtClientBot.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OtClientBot.WinApi;

namespace OtClientBot
{
    public partial class Debugger
    {
        static Breakpoint brSendingPacket; // Uma classe que representa um breakpoint
        static Breakpoint.BreakPointHandler OnSendPacket; // Delegate ( Function pointer em C++ )


        static void LoadSendingPacketBreakPoint(bool StartOnRun) // Essa função é usada pra carregar esse hook na memória
        {
            OnSendPacket = SendingPacketCallBack; // Esse é o callback sendo atribuido ao event handler "OnSendPacket"

            // Parametros : Em qual Address colocar o breakpoint, callback(s), bool que diz se deve ou não ativar o breakpoint
            brSendingPacket = new Breakpoint((uint)Address.Hooks.SendingPacket, OnSendPacket, StartOnRun);  
        } 
        static byte[] SendingOpCodeBlackList = new byte[] { 0x1E, 0x1D };  // Lista de OpCodes de packets que não devem ser logados

        static void SendingPacketCallBack(DEBUG_EVENT evt)
        {
            GetCtx(evt); // Pega o contexto dos registradores quando o breakpoint é ativado

            var packetAddres = ctx.Edx;  // No moment do breakpoint o address da esturtura de packet está no registrador Edx
            var DataStart = packetAddres + 0x1A;  // O array de bytes que representa o packet inicia-se no offset 0x1A
            var DataLen = Memory.ReadByte(DataStart - 0xA); // On memory, the packet size is a WORD, but for now we only need to read a byte.
            var rawPacket = Memory.ReadBytes(DataStart, DataLen); // Lê o packet


            #region Setter

            //Program.Log("Packet sent Address at: " + packetAddres.ToString("x8") );
            //Program.Log(BitConverter.ToString(Memory.ReadBytes(packetAddres, 32)).Replace("-"," "));


            //Client.LastPacketAddress = packetAddres; // Seta o Last Packet Address

            byte[] SeeItem = { 0x8C };

            if (rawPacket.Length == 9 && rawPacket[0] == SeeItem[0])
            {
                Client.LastSeenItemId = BitConverter.ToUInt16(rawPacket, 6);
            }




            #endregion



            #region Filter




            #endregion


            if (DataLen == 0 || SendingOpCodeBlackList.Contains(rawPacket[0])) // Se o tamanho do packet for 0 ou OpCode for blacklisted.
            {
                ContinueBreakPoint(brSendingPacket, true);  // Continua  a execução e mantém o breakpoint.
                return;
            }


            





            #region Loger


            // Then, log sending Packet
            string SentStuff = BitConverter.ToString(rawPacket).Replace("-", " "); /// Converte o array de bytes em string
            //Program.Log("Sent: " + SentStuff); // Loga os packets que foram enviados



            LogWritter.SentPackets.Enqueue(SentStuff);

            #endregion

            ContinueBreakPoint(brSendingPacket, true); // Continua  a execução e mantém o breakpoint.
        }



















    }
}
