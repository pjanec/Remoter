using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Management;

using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace Remoter
{
    public class SoftKiller
    {
        private static readonly ILog log = new ILog();

        AppDef appDef;
        Process proc;

        // If soft kill sequence is defined, we try to terminate the process using 
        // the actions from the sequence. Each action has a timeout defined; if timed out,
        // next action (presumably more severe) is executed.
        List<SoftKillAction> softKillSeq = new List<SoftKillAction>(); // soft kill attempts (before a hard kill is executed)
        int softKillSeqIndex = -1; // current killseq item just being processed

        public SoftKiller( AppDef appDef )
        {
            this.appDef = appDef;
            this.proc = null;
        }

        public void AddClose()
        {
            softKillSeq.Add( new KSI_Close() );
        }


        public bool IsDefined => softKillSeq.Count > 0;
        public bool IsRunning => softKillSeqIndex >= 0;

		public void Dispose()
		{
            Stop();
		}

        public void Start( Process proc )
        {
            this.proc = proc;

            // execute first of the key sequence
            if( softKillSeq.Count > 0 )
            {
                softKillSeqIndex = 0;
                softKillSeq[0].Execute( appDef, proc );
            }
            else // no kill seq defined
            {
                softKillSeqIndex = -1;
            }
        }

        public void Stop()
        {
            if( softKillSeqIndex >= 0 && softKillSeqIndex < softKillSeq.Count )
            {
                var ksi = softKillSeq[softKillSeqIndex];
                ksi.CleanUp();
            }
            softKillSeqIndex = -1;
            proc = null;
        }


        // returns false if no more soft kill action left and process still running
        public bool Tick()
        {
            // still executing the kill sequence?
            if( IsRunning )
            {
                var ksi = softKillSeq[softKillSeqIndex];

                // tick the kill action
                ksi.Tick();

                // process still running?
                if( !proc.HasExited )
                {
                    // timed out with current kill action?
                    if( ksi.TimedOut )
                    {
                        log.DebugFormat("SoftKill action {0} timed out", ksi.GetType().Name);
                        // cleanup the previous one
                        ksi.CleanUp();

                        // advance to next (and presumably more severe) kill action
                        softKillSeqIndex++;

                        if( softKillSeqIndex < softKillSeq.Count )
                        {
                            ksi = softKillSeq[softKillSeqIndex];
                            ksi.Execute( appDef, proc );
                        }
                        else  // no more kill action left
                        {
                            Stop();

                            // report we have failed to kill the process using the sequence...
                            return false;
                        }
                    }
                }
                else // process no longer running - we succeeded!
                {
                    Stop();
                }
            }
            return true;
        }

        class SoftKillAction
        {
            protected double timeout = -1;
            Stopwatch sw = new Stopwatch();

            public virtual void Tick()
            {
            }

            public bool TimedOut => timeout > 0 && sw.Elapsed.TotalSeconds > timeout;
            
            public SoftKillAction( double timeout = -1 )
            {
                this.timeout = timeout;
            }

            public virtual void Execute( AppDef appDef, Process proc )
            {
                // starts counting
                sw.Restart();
            }

            // called when kill action no longer needed (after it has been executed)
            public virtual void CleanUp()
            {
            }
        }

        class KSI_Close : SoftKillAction
        {
            public KSI_Close( double timeout = 10) : base(timeout)
            {
            }

			public override void Execute(AppDef appDef, Process proc)
			{
				base.Execute(appDef, proc);

                proc.CloseMainWindow();
			}
		}

        class KSI_Keys : SoftKillAction
        {
            [DllImport ("User32.dll")]
            static extern int SetForegroundWindow(IntPtr point);

            string keys;

            public KSI_Keys( string keys )
            {
                this.keys = keys;
            }
			public override void Execute(AppDef appDef, Process proc)
			{
				base.Execute(appDef, proc);

                IntPtr h = proc.MainWindowHandle;
                SetForegroundWindow(h);
                System.Windows.Forms.SendKeys.Send( keys );
			}
        }
 
    }

}
