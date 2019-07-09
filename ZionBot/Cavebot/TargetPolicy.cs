using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TargetPolicy
    {
        public string name { get; set; }
        public Enums.Priority priority { get; set; }
        public bool ignore { get; set; }
        public Enums.TargetingMode targetingMode { get; set; }
        public bool isDefault { get; set; }
        public bool isNotDefault { get { return !isDefault; } }


        public Enums.TargetingActions action
        {
            get
            {
                if (ignore) return Enums.TargetingActions.Ignore;
                else return Enums.TargetingActions.Attack;
            }

            set
            {
                ignore = Enums.TargetingActions.Ignore == value;
            }

        }


        public Array enumTargetingModes { get
            {
                return Enum.GetValues(typeof(Enums.TargetingMode));
            } }


        public Array enumLevelsOfPriority
        {
            get
            {
                return Enum.GetValues(typeof(Enums.Priority));
            }
        }

        public Array enumTargetingActions
        {
            get
            {
                return Enum.GetValues(typeof(Enums.TargetingActions));
            }
        }





        public TargetPolicy(string Name, Enums.Priority priority = 0, bool ignore = false, Enums.TargetingMode targetingMode = Enums.TargetingMode.None, bool isDefault = false)
        {
            this.isDefault = isDefault;
            this.name = Name;
            this.priority = priority;
            this.ignore = ignore;
            this.targetingMode = targetingMode;
        }


        public static string StringFromMode(Enums.TargetingMode Mode)
        {
            switch (Mode)
            {
                case (Enums.TargetingMode.Custom): return "Custom";
                case (Enums.TargetingMode.Follow): return "Follow";
                case (Enums.TargetingMode.FollowAtDistance): return "Distance";
                case (Enums.TargetingMode.None): return "None";
                case (Enums.TargetingMode.Stand): return "Stand";
                default: return string.Empty;
            }
        }

        public static Enums.TargetingMode ModeFromString(String Mode)
        {
            switch (Mode)
            {
                case "Follow": return Enums.TargetingMode.Follow;
                case "Custom": return Enums.TargetingMode.Custom;
                case "Distance": return Enums.TargetingMode.FollowAtDistance;
                case "None": return Enums.TargetingMode.None;
                case "Stand": return Enums.TargetingMode.Stand;
                default: return Enums.TargetingMode.Follow;
            }
        }




        public override string ToString()
        {
            string res = "[";
            res += this.name;
            res += ",";
            res += StringFromMode(this.targetingMode);
            res += ",";
            res += (ignore ? "ignore" : priority.ToString());
            res += "]";
            return res;

        }


        
    }
}
