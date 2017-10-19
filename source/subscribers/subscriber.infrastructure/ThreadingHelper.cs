using System;
using System.Threading;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public static class ThreadingHelper
    {
        /*

        public static void Post(this SynchronizationContext vm, Action action)
        {
            vm.Send(new SendOrPostCallback((o,t) => action();) );
        }

        public static void Post<T>(this DispatcherObject vm, Action<T> action, T t)
        {
            vm.Dispatcher.Invoke(DispatcherPriority.Normal, action, t);
        }

        public static void Post<T1, T2>(this DispatcherObject vm, Action<T1, T2> action, T1 t1, T2 t2)
        {
            vm.Dispatcher.Invoke(DispatcherPriority.Normal, action, t1, t2);
        }

        public static void Post<T1, T2, T3>(this DispatcherObject vm, Action<T1, T2, T3> action, T1 t1, T2 t2, T3 t3)
        {
            vm.Dispatcher.Invoke(DispatcherPriority.Normal, action, t1, t2, t3);
        }
        public static void Post<T1, T2, T3, T4>(this DispatcherObject vm, Action<T1, T2, T3, T4> action, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            vm.Dispatcher.Invoke(DispatcherPriority.Normal, action, t1, t2, t3, t4);
        }
         * */

        

    
        public static void QueueUserWorkItem(this object element, Action workItemCallback)
        {
            ThreadPool.QueueUserWorkItem(o => workItemCallback(), null);
        }


        public static void QueueUserWorkItem<T>(this object element, Action<T> workItemCallback, T t)
        {
            ThreadPool.QueueUserWorkItem(o => workItemCallback((T)o), t);
        }


        public static void QueueUserWorkItem<T1, T2>(this object element, Action<T1, T2> workItemCallback, T1 t1, T2 t2)
        {
            var state = new Tuple<T1, T2>(t1,t2);
            ThreadPool.QueueUserWorkItem(o =>
                                             {
                                                 var info = (Tuple<T1, T2>)o;
                                                 workItemCallback(info.Item1, info.Item2);
                                             }, state);
        }


        public static void QueueUserWorkItem<T1, T2, T3>(this object element, Action<T1, T2, T3> workItemCallback, T1 t1, T2 t2, T3 t3)
        {
            var state = new Tuple<T1, T2,T3>(t1,t2,t3);
            ThreadPool.QueueUserWorkItem(o =>
            {
                var info = (Tuple<T1, T2,T3>)o;
                workItemCallback(info.Item1, info.Item2, info.Item3);
            }, state);
        }



        public static void QueueUserWorkItem<T1, T2, T3, T4>(this object element, Action<T1, T2, T3, T4> workItemCallback, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var state = new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4);

            ThreadPool.QueueUserWorkItem(o =>
            {
                var info = (Tuple<T1, T2, T3, T4>)o;
                workItemCallback(info.Item1, info.Item2, info.Item3, info.Item4);
            }, state);
        }

        
    }

}