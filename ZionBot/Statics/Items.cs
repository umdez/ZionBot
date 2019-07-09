using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Items
    {

        public static ushort[] Containers = { 1987, 1991, 1992, 1993, 1994, 1995, 1996, 1997, 3939, 5927, 5950, 1988, 1998, 1999, 2000, 2001, 2002, 2003, 2004, 2365, 3940, 3960, 5926, 5949, 2595, 2596 };

        public static ushort[] Foods = { 2666, 2671, 2689, 2691, 3682, 2696, 2667, 2679, 2686, 2668, 2690, 2684, 2681, 2673, 2674, 2676, 2677, 2685, 2683, 2680, 2675, 2687, 2695, 2787, 2788, 2789, 2790, 2791, 2792, 2795, 2796, 2678, 2672, 2669, 2670, 2688, 2328, 5678, 6541, 6542, 6543, 6544, 6545 };

        public static ushort[] StackableItems = { 2148 };

        public static ushort[] ExceptionStackable = { 2006 };

        public struct Rune
        {
            string Spell;
            int ManaCost;
            ushort RuneId;
        }

        public enum Tools : ushort
        {
            Rope = 2120,
            Shovel = 2554

        }



    }
}
