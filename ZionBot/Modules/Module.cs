using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace OtClientBot
{
    public class Module
    {

        private Thread ModuleThread;

        public ThreadStart ThreadEntryPoint;

        //public Action AtPause;
        //public Action AtResume;
        public Action AtStop;
        public Action AtStart;




        public Module()
        {
        }

        
        public virtual bool isAlive()
        {
            if (this.ModuleThread == null)
            {
                return false;
            }

            return this.ModuleThread.IsAlive;


        }

        //public virtual bool isRunning()
        //{
        //    return (this.ModuleThread.ThreadState == ThreadState.Running) ;
        //}

        public virtual void WaitPing(int milisseconds = 0)
        {
            Client.WaitPing(milisseconds);
        }

        public virtual void Wait(int milisseconds)
        {
            Thread.Sleep(milisseconds);
        }

        public void Start()
        {
            AtStart?.Invoke();
            if (ModuleThread != null)
            {
                if (ModuleThread.ThreadState == ThreadState.Running) return;
            }


            if (ThreadEntryPoint != null)
            {  
                ModuleThread = new Thread(ThreadEntryPoint);
                ModuleThread.IsBackground = true;
                ModuleThread.Start();
            }
        }


        public void Stop()
        {
            AtStop?.Invoke();
            if (this.ModuleThread != null)
            {
                if (ModuleThread.ThreadState==ThreadState.Suspended) ModuleThread.Resume();
                if (ModuleThread.IsAlive)  ModuleThread.Abort();
            }
        }

        //public void Pause()
        //{
        //    AtPause?.Invoke();
        //    if (this.ModuleThread != null && this.ModuleThread.ThreadState == ThreadState.Running)
        //    {
        //        ModuleThread.Suspend();
        //    }
        //}

        //public void Resume()
        //{
        //    AtResume?.Invoke();
        //    if (this.ModuleThread != null && this.ModuleThread.ThreadState == ThreadState.Suspended)
        //    {
        //         ModuleThread.Resume();
        //    }
        //}


        





    }
}
