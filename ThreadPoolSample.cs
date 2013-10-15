using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyAndParallelismRecap
{
    public class ThreadPoolSample
    {
        /*
           
         * Abstraction providing a set of threads that can be used to execute tasks, the size of this set
            automatically scales as the work load varies.
           
         * Allows the cost of creating threads to be spread across the process lifetime.
            
         * All threads in the pool are background threads, they will not keep the application alive if they
            have work scheduled and all foreground threads return.
         
         * The thread pool is the default scheduler for the TPL. 
         
         * A process has a single thread pool shared between all it's AppDomains
         
         */

        public void UsingTheThreadPool()
        {
            // the WaitCallback delegate
            WaitCallback waitCallback = new WaitCallback((object parameter) => {});

            // queue work using the static QueueUserWorkItem
            ThreadPool.QueueUserWorkItem(waitCallback);

            // you can pass state to the waitback too
            ThreadPool.QueueUserWorkItem(waitCallback, new object());
        }
    }
}
