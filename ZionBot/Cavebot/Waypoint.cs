using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot
{
    public class Waypoint : INotifyPropertyChanged


    {

        public Enums.Action Action;
        public Location Location;
        private int _Errors = 0;
        public int ErrorCount { get { return _Errors; } set { _Errors = value; OnDisplayPropertyChanged(); } }
        public int SuccessCount = 0;
        public int ExecutionCount = 0;

        public string Argument = "";

        public override string ToString()
        {
            {
                string res = "[";

                res += StringFromAction(this.Action);
                res += ",";
                res += Location.X.ToString();
                res += ",";
                res += Location.Y.ToString();
                res += ",";
                res += Location.Z.ToString();
                res += ",";
                res += Argument;
                res += "]";
                //res += "#";
                //res += ErrorCount.ToString();

                return res;

            }
        }

        public string StringFromAction(Enums.Action Action)
        {
            switch (Action)
            {
                case (Enums.Action.Walk): return "Walk";
                case (Enums.Action.Ladder): return "Ladder";
                case (Enums.Action.Shovel): return "Shovel";
                case (Enums.Action.Use): return "Use";
                case (Enums.Action.Rope): return "Rope";
                case (Enums.Action.Stand): return "Stand";
                case (Enums.Action.Script): return "Script";
                default: return string.Empty;
            }
        }

        public Enums.Action ActionFromString(String Action)
        {
            switch (Action)
            {
                case "Walk": return Enums.Action.Walk;
                case "Ladder": return Enums.Action.Ladder;
                case "Shovel": return Enums.Action.Shovel;
                case "Use": return Enums.Action.Use;
                case "Rope": return Enums.Action.Rope;
                case "Stand": return Enums.Action.Stand;
                case "Script": return Enums.Action.Script;
                default: return Enums.Action.Walk  ;
            }
        }

        public Waypoint(Enums.Action Action, Location Location, string Argument = "")
        {
            this.Action = Action;
            this.Location = Location;
            this.Argument = Argument;
        }

        public Waypoint(string Action, Location Location, string Argument = "")
        {
            this.Action = ActionFromString(Action);
            this.Location = Location;
            this.Argument = Argument;
        }

        public Waypoint(string WaypointString)
        {
            WaypointString = WaypointString.Split('#')[0];
            WaypointString = WaypointString.Replace(" ", "").Replace("[", "").Replace("]", "").Trim();
            string[] Values = WaypointString.Split(',');

            this.Action = ActionFromString(Values[0]);
            this.Location = new Location(Int32.Parse(Values[1]), Int32.Parse(Values[2]), Int32.Parse(Values[3]));
            this.Argument = Values[4];
        }


        public string DisplayName
        {
            get
            {
                return this.ToString();
            }
        }
        private int _NodeRadius = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        void OnDisplayPropertyChanged()
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));
        }

        public int NodeRadius
        {
            get
            {           
                int radius = _NodeRadius;


                // if _NodeRadius is Negative then try to parse argument to get it from there.
                if (_NodeRadius < 0 && string.IsNullOrWhiteSpace(Argument)==false &&  Int32.TryParse(Argument, out radius))
                {
                    if (radius > 5)
                    {
                        radius = 5;
                    }
                    else if (radius < 0)
                    {
                        radius = 0;
                    }
                }

                // If it is still negative then throw an exception.
                if (radius < 0) throw new Exception("Node radius must be set");


                return radius;
            }

            set
            {
                _NodeRadius = value;
            }

        }








    }
}
