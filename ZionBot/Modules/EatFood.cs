using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class EatFood : Module
    {

        public EatFood()
        {
            base.ThreadEntryPoint = EatFoodThread;
        }
        

        public void EatFoodThread()
        {

            while (Player.IsOnline)
            {
                if (Player.isHungry && !Player.isProtectionZone)
                {
                    foreach (ushort ItemId in Items.Foods)
                    {
                        var food = Iventory.FindItem(ItemId);

                        if (food != null)
                        {
                            Player.UseItem(food);
                            break;
                        }
                    }
                }
                Wait(2000);
            }

        }









    }
}
