using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrencyAndParallelismRecap
{
    public  class TaskSample
    {
        public void CreateTask()
        {
            Action action = () => { Console.WriteLine("Some Work"); };

            Func<bool> function = () => true;

            // start a simple task
            Task task = Task.Run(action);

            // OR
            task = new Task(action);
            task.Start();

            // task that returns a result
            Task<bool> taskWithResult = Task.Run<bool>(function);

            // OR
            taskWithResult = new Task<bool>(function);
            taskWithResult.Start();

            // access the result - if the task hasn't completed it will block until the result becomes available
            var result = taskWithResult.Result;
        }

        public void WaitingForTasks()
        {
            Action action = () => { };

            Task task1 = Task.Run(action), task2 = Task.Run(action), task3 = Task.Run(action);
            Task<int> task4 = Task.Run<int>(() => 1);

            // waiting for a single task
            task1.Wait();

            // accessing the Result property of a Task with cause it to block until it's completed if its still running.
            var result = task4.Result;

            // you can wait for a set of tasks using the following

             // waits for all tasks to complete before continuing
            Task.WaitAll(task4, task3, task1, task2);

            // waits for one task to complete, returning the index of the completed task
            Task.WaitAny(task4, task3, task1);
        }

        // We can use continutions to start another task off as soon as one task completes
        public void Starting_A_New_Task_When_One_Completes()
        {
            // start a task 
            Task firstTask = Task.Run(() => { });

            // when first task conmpletes, second task will be started, 
            // the parameter passed into the action is first task
            Task secondTask = firstTask.ContinueWith((task) => { });

            // we can also do this fluently
            Task fluent = Task.Run(() => { })
                .ContinueWith((task) => { })
                .ContinueWith((task) => { });

            // well can access the result the first task in the continuation
            Task withResult =
                Task.Run<int>(() => { return 5; })
                .ContinueWith<int>((task) => { return task.Result + 5; });
        }

        public void Create_A_Continuation_For_A_Collection_Of_Tasks()
        {
            Task task1 = Task.Run(() => { }),
                task2 = Task.Run(() => { }),
                task3 = Task.Run(() => { }),
                task4 = Task.Run(() => { });

            // wait for all tasks to complete before starting a continuation
            Task.WhenAll(task3, task1, task2, task4)
                .ContinueWith(task => { }); 
            // any exceptions thrown by the tasks will be wrapped up into an aggregate exception and the resulting a task will be in a "Faulted" state
            // if any of the tasks where cancelled by none faulted, then the resulting task will be in "Canceled" state.
            // otherwise the resulting task will have a "RanToCompletion" state

            Task<int> task5 = Task.Run(() => 1),
              task6 = Task.Run(() => 2),
              task7 = Task.Run(() => 3),
              task8 = Task.Run(() => 4);

            Task.WhenAll<int>(task5, task6, task7, task8)
                .ContinueWith(task => task.Result);
            // when the tasks return a result the resulting task's result will be an array of the result in the order that they were waited upon 
            // e.g. [ task5.Result, task6.Result, task7.Result, task8.Result ]

        }

        // we can also conditional start a task on the completion of a previous task, depending on the state of the previous task
        public void Conditional_Starting_A_Task_From_Another()
        {
            Task.Run(() =>
            {
                // hard work
            })
            .ContinueWith(task => { }, TaskContinuationOptions.OnlyOnRanToCompletion) // only run continuation when first task successfully completed
            .ContinueWith(task => { }, TaskContinuationOptions.OnlyOnFaulted)         // only run continuation when first task threw an exception
            .ContinueWith(task => { }, TaskContinuationOptions.OnlyOnCanceled);       // only run continuation when first task was canceled.

            // there are more continuation options
        }



    }
}
