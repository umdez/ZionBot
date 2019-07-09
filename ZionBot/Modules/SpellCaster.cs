using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class SpellCaster : Module
    {

        public SpellCaster()
        {
            base.ThreadEntryPoint = SpellCasterThread;
        }

        public bool HealUH = false;
        public int HPUH;
        public ushort UHID = 2273;

        public bool HealHigh = false;
        public string HighSpell;
        public int HPHI;
        public int MPHICOST;

        public bool HealLow = false;
        public string LowSpell;
        public int HPLO;
        public int MPLOCOST;

        public bool ManaTrain = false;
        public string ManaTrainSpell;
        public int MANATRAINMP;

        public void SpellCasterThread()
        {
            while (Player.IsOnline)
            {
                Wait(10);

                if (HealLow && Player.HP < HPLO && Player.MP > MPLOCOST)
                {
                    Player.Cast(LowSpell);
                    Wait(1000);
                    continue;
                }else if ( HealHigh && Player.HP < HPHI && Player.MP > MPHICOST)
                {
                    Player.Cast(HighSpell);
                    Wait(1000);
                    continue;
                }else if(HealUH && Player.HP < HPUH)
                {
                    var UH = Iventory.FindItem(UHID);
                    if (UH == null)
                    {
                        HealUH = false;
                    }else
                    {
                        Player.UseWith(UH, Player.Location);
                        Wait(1000);
                    }
                    continue;
                }
                else if (ManaTrain && Player.MP > MANATRAINMP)
                {
                    Player.Cast(ManaTrainSpell);
                    Wait(1000);
                    continue;
                }
                
               
            }

        }









    }
}
