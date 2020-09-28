using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Core
{
    public class ThreadPool
    {
        #region Properties

        public int MaxThreadCount { get; set; }

        #endregion Properties

        #region Fields

        private List<ActionArgsBase> actions = new List<ActionArgsBase>();
        private object objSync = new object();
        private object objSyncActions = new object();
        private int cntThread = 0;

        private const int MinThreadCount = 3;

        #endregion Fields

        #region Ctor

        public ThreadPool()
        {
            MaxThreadCount = Environment.ProcessorCount * 3 > MinThreadCount ? Environment.ProcessorCount * 3 : MinThreadCount;
            //MaxThreadCount = 6;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void Start(Action<object> callback, object state)
        {
            lock (objSyncActions)
                actions.Add(new ActionArgs<object>() { Action = callback, Args = state });

            TryStartupThread();
        }

        public void Start<T>(Action<T> callback, T state)
        {
            lock (objSyncActions)
                actions.Add(new ActionArgs<T>() { Action = callback, Args = state });

            TryStartupThread();
        }

        public void Start(Action callback, bool useThreadPool = true)
        {
            if (useThreadPool)
            {
                lock (objSyncActions)
                    actions.Add(new ActionArgs() { Action = callback });

                TryStartupThread();
            }
            else
            {
                Thread t = new Thread(new ThreadStart(() => callback.Invoke())) { IsBackground = true };
                t.Start();
            }
        }

        public void Start(Action callback, int maxThreadCount)
        {
            //if (useThreadPool)
            //{
            lock (objSyncActions)
                actions.Add(new ActionArgs() { Action = callback });

            TryStartupThread(maxThreadCount);
        }

        public void Start(Action callback, ApartmentState state)
        {
            lock (objSyncActions)
                actions.Add(new ActionArgs() { Action = callback });

            TryStartupThread();
        }

        #endregion Methods - Public

        #region Methods - Private

        private void TryStartupThread(int maxThreadCount = -1)
        {
            maxThreadCount = maxThreadCount <= 0 ? MaxThreadCount : maxThreadCount;
            maxThreadCount = maxThreadCount <= 0 ? MinThreadCount : maxThreadCount;

            lock (objSync)
                if (cntThread > maxThreadCount)
                    return;

            Thread t = new Thread(new ThreadStart(ThreadProc)) { IsBackground = true };
            //t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }

        private void ThreadProc()
        {
            while (actions.Count > 0)
            {
                lock (objSync)
                    cntThread++;

                do
                {
                    var args = TryGetAction();
                    if (args == null)
                        break;

                    args.Invoke();
                } while (true);

                lock (objSync)
                    cntThread--;
            }
        }

        private ActionArgsBase TryGetAction()
        {
            lock (objSyncActions)
            {
                if (actions.Count == 0)
                    return null;

                var args = actions[0];
                actions.RemoveAt(0);
                return args;
            }
        }

        #endregion Methods - Private

        #endregion Methods

        #region Methods - Class

        private class ActionArgsBase
        {
            #region Methods

            public virtual void Invoke()
            {
            }

            #endregion Methods
        }

        private class ActionArgs : ActionArgsBase
        {
            #region Properties

            public Action Action { get; set; }

            #endregion Properties

            #region Methods

            public override void Invoke()
            {
                Action.Invoke();
            }

            #endregion Methods
        }

        private class ActionArgs<T> : ActionArgsBase
        {
            #region Properties

            public Action<T> Action { get; set; }
            public T Args { get; set; }

            #endregion Properties

            #region Methods

            public override void Invoke()
            {
                Action.Invoke(Args);
            }

            #endregion Methods
        }

        #endregion Methods - Class
    }
}