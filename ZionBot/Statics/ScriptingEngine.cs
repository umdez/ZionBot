using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtClientBot
{


    class  StringBuilderStream : Stream
    {
        private StringBuilder sb;

        public StringBuilderStream(StringBuilder sb)
        { this.sb = sb; }

        public override bool CanRead { get { return false; } }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite { get { return true; } }

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            //throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.sb.Append(Encoding.UTF8.GetString(buffer, offset, count));
        }
    }


    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class RunningScript
    {

        public delegate void StatusDelegate(Status s);


        public StatusDelegate OnStatusChanged;


        private StringBuilder OutputStringBuilder;

        public string output { get { return OutputStringBuilder.ToString(); } }


        public string scriptContent { get; private set; }


        public string name { get; set; }

        public Status status { get; private set; }


        private ScriptEngine engine;

        private ScriptScope scope;


        private Thread ExecutionThread;


               

        public enum Status
        {
            Running,
            Error,
            Finished,
            Stopped,
            Ready
        }




        public RunningScript(string scriptToRun, ScriptEngine engine, ScriptScope scope, string Name = "", StatusDelegate OnStatusChanged = null)
        {
            this.scriptContent = scriptToRun;
            this.OutputStringBuilder = new StringBuilder();
            this.OnStatusChanged = OnStatusChanged;


            this.engine = engine;
            this.scope = scope;

            if (string.IsNullOrEmpty(Name)) this.name = "<Unnamed Script>";
            else this.name = Name;

            // Redireciona o stdOut para o StringBuilder.
            this.engine.Runtime.IO.SetOutput(new StringBuilderStream(this.OutputStringBuilder), Encoding.UTF8);

            // Redireciona o stdErr para o StringBuilder também;
            //this.engine.Runtime.IO.SetErrorOutput(new StringBuilderStream(this.OutputStringBuilder), Encoding.UTF8);



            var source = this.engine.CreateScriptSourceFromString(scriptToRun);
            
            this.ExecutionThread = new Thread(new ThreadStart(() => 
            
            {
                this.status = Status.Running;
                if (this.OnStatusChanged != null) this.OnStatusChanged(this.status);

                try
                {
                    source.Execute(scope);
                    this.status = Status.Finished;
                    if (this.OnStatusChanged != null) this.OnStatusChanged(this.status);

                }
                catch (Exception e)
                {
                    if (e.Message != "Thread was being aborted.")
                    {
                        OutputStringBuilder.Append("Error:\n\t"+e.Message);

                        this.status = Status.Error;
                        if (this.OnStatusChanged != null) this.OnStatusChanged(this.status);
                    }
                }

            }
            
            
            
            
            ));

            this.ExecutionThread.IsBackground = true;


            this.ExecutionThread.Start();

            


        }




        public void Stop()
        {
            ExecutionThread.Abort();
            this.status = Status.Stopped;
            if (this.OnStatusChanged != null) this.OnStatusChanged(this.status);


        }











    }



    public class ScriptingEngine
    {

        //internal static ObservableCollection<RunningScript> runningScripts { get; set; }


        public static List<RunningScript> ExecutedScripts = new List<RunningScript>();


        public ScriptingEngine()
        {
        }



        internal static void PopulateScope(ScriptScope scope)
        {
            //// Interfaces

            scope.SetVariable("Modules", new Modules());
            scope.SetVariable("Player", new Player());
            scope.SetVariable("BattleList", new BattleList());
            scope.SetVariable("MiniMap", new Minimap());
            scope.SetVariable("Iventory", new Iventory());
            scope.SetVariable("Items", new Items());
            scope.SetVariable("GameMap", new GameMap());
            scope.SetVariable("Client", new Client());
            scope.SetVariable("Draw", new Draw());
            scope.SetVariable("Cavebot", new Cavebot._Cavebot());

            scope.SetVariable("Scripter", new ScriptingEngine());




            /// Engines
            scope.SetVariable("Find", new Find());
            scope.SetVariable("Memory", new Memory());
            scope.SetVariable("Debugger", new Debugger());
            scope.SetVariable("Post", new Post());
            scope.SetVariable("Utils", new Utils());


            //// Objects
            scope.SetVariable("Packet", typeof(Packet));
            scope.SetVariable("Location", typeof(Location));
            scope.SetVariable("Item", typeof(Item));
            scope.SetVariable("Creature", typeof(Creature));
            scope.SetVariable("Container", typeof(Container));
            scope.SetVariable("Waypoint", typeof(Cavebot.Waypoint));
            scope.SetVariable("Walker", typeof(Cavebot.Walker));
            scope.SetVariable("Targeting", typeof(Cavebot.Targeting));
            scope.SetVariable("AutoLoot", typeof(Cavebot.AutoLoot));


            scope.SetVariable("PathFinder", typeof(Utilities.PathFinder));


            //// Types
            scope.SetVariable("WaypointList", typeof(List<Cavebot.Waypoint>));
        }


        public static RunningScript RunScript(string Script, string Name = "",RunningScript.StatusDelegate OnStatusChanged = null)
        {

            var engine = IronPython.Hosting.Python.CreateEngine();
            var scope = engine.CreateScope();

            PopulateScope(scope); // Popula o escopo.

            var rScript = new RunningScript(Script, engine, scope, Name,OnStatusChanged);

            ExecutedScripts.Add(rScript);

            return rScript;
                 


        }







    }
}
