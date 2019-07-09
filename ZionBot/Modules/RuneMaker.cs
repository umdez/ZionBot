using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class RuneMaker : Module
    {
        // Need to be tested. 


        public static ushort BlankRuneId = 2260;


        public string RuneSpell = null;
        public int ManaCost = 0;   

        public Iventory.EquipSlot Hand = Iventory.EquipSlot.RightHand;


        private int lastContainerIndex = 0;




        public RuneMaker() 
        {
            base.ThreadEntryPoint = RuneMakerThread;
            base.AtStart += Setup;
        }

        

        private void Setup()
        {
            if (RuneSpell == null || ManaCost == 0)
            {
                Client.Output("Please, configure Rune maker first.");
                this.Stop();
            }else
            {
                Client.Output(string.Format("Running RuneMaker. Spell: {0} Mana Cost: {1}",RuneSpell,ManaCost));
            }

            //Iventory.OpenAllContainers;            
        }

        public void RuneMakerThread()
        {
            while(Player.IsOnline)
            {

                if (Player.MP > ManaCost) // Check Mana , if there is enough mana, then make the rune.
                {
                    var BlankRune = Iventory.FindItem(BlankRuneId); // Find Blank Rune
                    if (BlankRune != null) // If there is Blank Rune
                    {

                        EmptyHand();
                        MoveBlankToHand(BlankRune);
                        Player.Cast(RuneSpell);
                        WaitPing();
                        MoveRuneToBackpack();

                        if (Iventory.FindItem(BlankRuneId)== null)
                        {
                            WhenNoBlankRunes();                            
                        }

                    }
                    else // If no blank runes are found.
                    {
                        WhenNoBlankRunes();
                    }
                }

                Wait(2000);
            }

        }

        private void WhenNoBlankRunes()  // Do something when out of blank runes.

        {
            this.Stop();
        }

        private void MoveRuneToBackpack()
        {
            var lastContainer = Iventory.GetContainerByIndex(lastContainerIndex);

            var EmptySlot = lastContainer.GetEmptySlot();

            if (EmptySlot == null) EmptySlot = Iventory.FindEmptySlot(); // If not found an empty slot on the last container then try to find any empty slot.
            
            if (EmptySlot==null)
            {
                Client.Output("Could not find any empty slot. RuneMaker is stopping.");
            }
            
            var Rune = Iventory.GetSlot(Hand); // Get the rune on the hand.
            
            Player.MoveItem(Rune, EmptySlot);
            WaitPing();
        }

        private void MoveBlankToHand(Item blankRune)
        {
            lastContainerIndex = blankRune.Location.Y - 0x80; // Y - 0x40   stands for the backpack index when it's related to containers on iventory.

            var HandLocation = Iventory.SlotLocation(Hand);

            Player.MoveItem(blankRune, HandLocation);
            WaitPing();
            


        }

        public void EmptyHand()
        {
            var HandSlot = Iventory.GetSlot(Hand);

            if (HandSlot.isItem)
            {
                var EmptySlot = Iventory.FindEmptySlot();

                if(EmptySlot==null)
                {
                    Client.Output("Could not find empty slot. Stopping RuneMaker.");

                    this.Stop();
                }

                Player.MoveItem(HandSlot, EmptySlot, HandSlot.Count);
                WaitPing();
            }

        }





    }
}
