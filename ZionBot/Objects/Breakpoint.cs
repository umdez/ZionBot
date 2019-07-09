using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtClientBot
{
    public class Breakpoint
    {
        public delegate void BreakPointHandler(WinApi.DEBUG_EVENT evt);

        public uint Address;
        public byte OpCode;

        public BreakPointHandler Handler;

        public static Dictionary<uint, Breakpoint> Breakpoints = new Dictionary<uint, Breakpoint>();
        public static List<Breakpoint> AllBreakPoints = new List<Breakpoint>();


        public bool Activated
        {
            get
            {
                if (Memory.ReadByte(this.Address) == 0xCC)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    Memory.WriteByte(this.Address, 0xCC);
                else
                    Memory.WriteByte(this.Address, this.OpCode);
            }
        }


        public Breakpoint(uint Address, BreakPointHandler Handler, bool Activate = false)
        {
            AllBreakPoints.Add(this);
            Breakpoints.Add(Address, this);

            this.Handler = Handler;
            this.Address = Address;
            this.OpCode = Memory.ReadByte(Address);



            this.Activated = Activate;

        }

        public Breakpoint(uint Address, bool Activate = false)
        {
            AllBreakPoints.Add(this);
            this.Address = Address;
            this.OpCode = Memory.ReadByte(Address);


            this.Activated = Activate;
        }


        public static void RemoveAllBreakPoints()
        {
            foreach (Breakpoint b in AllBreakPoints)
            {
                b.Activated = false;
            }
        }

    }
}
