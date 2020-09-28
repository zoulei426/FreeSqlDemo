using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public abstract class TaskQueue
    {
        #region Properties

        public bool UseThreadPool { get; set; }

        //public int MaxThreadCount { get; set; }

        public ThreadPool Pool { get; set; }

        public bool IsAlive { get; private set; }
        public bool IsBusy { get; private set; }

        #endregion Properties

        #region Fields

        private object objSync = new object();
        private object objSyncCount = new object();

        private int cntThread;

        private List<TaskDelegateMetadata> tasks;
        private ITask currentTask = null;
        //private Thread currentThread = null;

        #endregion Fields

        #region Ctor

        public TaskQueue()
        {
            UseThreadPool = true;
            //MaxThreadCount = -1;
            tasks = new List<TaskDelegateMetadata>();
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void Abort()
        {
            lock (tasks)
            {
                tasks.Clear();
                if (currentTask != null)
                    currentTask.Stop();

                //if (currentThread != null)
                //    currentThread.Abort();
            }
        }

        public void Cancel()
        {
            lock (tasks)
            {
                tasks.Clear();
                if (currentTask != null)
                    currentTask.Stop();
            }
        }

        public void Clear()
        {
            lock (tasks)
                tasks.Clear();
        }

        public void Interrupt()
        {
            lock (tasks)
            {
                if (currentTask == null)
                    return;

                currentTask.Stop();
            }
        }

        public void Do(TaskDelegateMetadata meta)
        {
            AddTask(false, meta.Go, meta.Alert, meta.ProgressChanged,
                meta.Started, meta.Ended, meta.Completed, meta.Stopped, meta.Terminated, meta.UserState);
        }

        public void Do(
            Action<TaskGoEventArgs> go,
            Action<TaskCompletedEventArgs> completed = null,
            Action<TaskTerminatedEventArgs> terminated = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> ended = null,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskStoppedEventArgs> stopped = null,
            object userState = null)
        {
            AddTask(false, go, alert, progressChanged, started, ended, completed, stopped, terminated, userState);
        }

        public void DoImmediately(TaskDelegateMetadata meta)
        {
            AddTask(true, meta.Go, meta.Alert, meta.ProgressChanged,
                meta.Started, meta.Ended, meta.Completed, meta.Stopped, meta.Terminated, meta.UserState);
        }

        public void DoImmediately(
            Action<TaskGoEventArgs> go,
            Action<TaskCompletedEventArgs> completed = null,
            Action<TaskTerminatedEventArgs> terminated = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> ended = null,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskStoppedEventArgs> stopped = null,
            object userState = null)
        {
            AddTask(true, go, alert, progressChanged, started, ended, completed, stopped, terminated, userState);
        }

        public void DoWithInterruptCurrent(TaskDelegateMetadata meta)
        {
            Interrupt();
            AddTask(false, meta.Go, meta.Alert, meta.ProgressChanged,
                meta.Started, meta.Ended, meta.Completed, meta.Stopped, meta.Terminated, meta.UserState);
        }

        public void DoWithInterruptCurrent(
            Action<TaskGoEventArgs> go,
            Action<TaskCompletedEventArgs> completed = null,
            Action<TaskTerminatedEventArgs> terminated = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> ended = null,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskStoppedEventArgs> stopped = null,
            object userState = null)
        {
            Interrupt();
            AddTask(false, go, alert, progressChanged, started, ended, completed, stopped, terminated, userState);
        }

        public void DoImmediatelyWithInterruptCurrent(TaskDelegateMetadata meta)
        {
            Interrupt();
            AddTask(true, meta.Go, meta.Alert, meta.ProgressChanged,
                meta.Started, meta.Ended, meta.Completed, meta.Stopped, meta.Terminated, meta.UserState);
        }

        public void DoImmediatelyWithInterruptCurrent(
            Action<TaskGoEventArgs> go,
            Action<TaskCompletedEventArgs> completed = null,
            Action<TaskTerminatedEventArgs> terminated = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> ended = null,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskStoppedEventArgs> stopped = null,
            object userState = null)
        {
            Interrupt();
            AddTask(true, go, alert, progressChanged, started, ended, completed, stopped, terminated, userState);
        }

        #endregion Methods - Public

        #region Methods - Thread

        private void TaskProc()
        {
            while (tasks.Count > 0)
            {
                try
                {
                    lock (objSyncCount)
                        cntThread++;

                    lock (objSync)
                        InnerTaskProc();
                }
                finally
                {
                    lock (objSyncCount)
                        cntThread--;
                }
            }
        }

        private void InnerTaskProc()
        {
            //currentThread = Thread.CurrentThread;
            IsAlive = true;

            while (tasks.Count > 0)
            {
                var task = GetTask();
                if (task == null)
                    continue;

                IsBusy = true;
                currentTask.Start();
                IsBusy = false;

                lock (tasks)
                    currentTask = null;
            }

            IsAlive = false;
        }

        private ITask GetTask()
        {
            if (tasks.Count == 0)
                return null;

            ITask task = null;

            lock (tasks)
            {
                TaskDelegateMetadata meta = null;

                if (tasks.Count != 0)
                {
                    meta = tasks[0];
                    tasks.RemoveAt(0);
                    task = CreateTask(meta);
                    currentTask = task;
                }
            }

            return task;
        }

        private ITask CreateTask(TaskDelegateMetadata meta)
        {
            var task = Task.Create(
                go =>
                {
                    OnGo(meta, go);
                },
                alert =>
                {
                    OnAlert(meta, alert);
                },
                progressChanged =>
                {
                    OnProgressChanged(meta, progressChanged);
                },
                started =>
                {
                    OnStarted(meta, started);
                },
                ended =>
                {
                    OnEnded(meta, ended);
                },
                completed =>
                {
                    OnCompleted(meta, completed);
                },
                stopped =>
                {
                    OnStopped(meta, stopped);
                },
                terminated =>
                {
                    OnTerminated(meta, terminated);
                });

            task.Argument.UserState = meta.UserState;
            return task;
        }

        #endregion Methods - Thread

        #region Methods - Override

        protected virtual void OnTerminated(TaskDelegateMetadata meta, TaskTerminatedEventArgs terminated)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnStopped(TaskDelegateMetadata meta, TaskStoppedEventArgs stopped)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnCompleted(TaskDelegateMetadata meta, TaskCompletedEventArgs completed)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnEnded(TaskDelegateMetadata meta, TaskEndedEventArgs ended)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnStarted(TaskDelegateMetadata meta, TaskStartedEventArgs started)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnProgressChanged(TaskDelegateMetadata meta, TaskProgressChangedEventArgs progressChanged)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnAlert(TaskDelegateMetadata meta, TaskAlertEventArgs alert)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnGo(TaskDelegateMetadata meta, TaskGoEventArgs go)
        {
            throw new NotImplementedException();
        }

        #endregion Methods - Override

        #region Methods - Private

        private void AddTask(bool im,
            Action<TaskGoEventArgs> go,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> ended = null,
            Action<TaskCompletedEventArgs> completed = null,
            Action<TaskStoppedEventArgs> stopped = null,
            Action<TaskTerminatedEventArgs> terminated = null,
            object userState = null)
        {
            var meta = new TaskDelegateMetadata()
            {
                Go = go,
                Terminated = terminated,
                Stopped = stopped,
                Started = started,
                Completed = completed,
                Ended = ended,
                ProgressChanged = progressChanged,
                Alert = alert,
                UserState = userState
            };

            lock (tasks)
            {
                if (im)
                    tasks.Insert(0, meta);
                else
                    tasks.Add(meta);
            }

            lock (objSyncCount)
                if (cntThread < 1)
                {
                    if (Pool == null)
                        ThreadWorkstaion.Start(new Action(() => TaskProc()), UseThreadPool);
                    else
                        Pool.Start(new Action(() => TaskProc()), UseThreadPool);
                }
        }

        #endregion Methods - Private

        #endregion Methods
    }
}