using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyAndParallelismRecap
{
    public class ThreadSamples
    {
        public void CreateSimpleThread()
        {
            // the work of to 

            Action doWork = () => { 
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId); 
                // once the thread returns from it's entry method it reaches the end of it's lifetime
            };
                        
            // create the delegate
            ThreadStart threadStart = new ThreadStart(doWork);

            // create the thread
            Thread thread = new Thread(threadStart);

            // span
            thread.Start();
        }

        public void CreateThreadWithData()
        {
            Action<object> doWork = 
                (parameter) =>
                {
                    int? number = parameter as int?;

                    Console.WriteLine("[ManagedThreadId: {0}] {1}", Thread.CurrentThread.ManagedThreadId, number);
                };

            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(doWork);

            Thread thread = new Thread(threadStart);
            
            thread.Start(1);
        }

        public void ThreadProperties()
        {
            Action doWork = () => { };

            Thread thread = new Thread(new ThreadStart(doWork));

            thread.IsBackground = true; // if all Foreground threads complete before this thread, terminate the process anyway.

            var isAlive = thread.IsAlive; // instanteous snapshot of the thread's execution state

            var name = thread.Name; // set thread name +Debuggability

            var priority = thread.Priority; // determines the amount of CPU time the thread gets

            var state = thread.ThreadState; // details of the thread's state

            // CurrentPrincipal, CurrentCulture, CurrentUICulture => other interesting propreties
        }

        public void WaitingForAThreadToComplete()
        {
            Action doWork = () => { };

            Thread thread = new Thread(new ThreadStart(doWork));

            thread.Start();

            thread.Join(); // blocks until the thread completes execution
        }

        public void AbortingAThread()
        {

            Action doWork = () => { Thread.Sleep(1000); };

            Thread thread = new Thread(new ThreadStart(doWork));

            thread.Start();

            // avoid, use cooperative polling instead. Can lead to corruption of state
            thread.Interrupt(); 
            thread.Abort(); // raising ThreadAbortException on thread
        }
    }
}
