using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class TaskContainer : CDObject
    {
        #region Properties

        public virtual TaskQueue Queue { get; set; }

        #endregion Properties

        #region Ctor

        public TaskContainer(TaskQueue taskQueue)
        {
            if (taskQueue == null)
                throw new ArgumentNullException("taskQueue");

            Queue = taskQueue;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public void Abort()
        {
            Queue.Abort();
        }

        public void Cancel()
        {
            Queue.Cancel();
        }

        public virtual void Reset(object userState = null)
        {
            Queue.Cancel();
            Queue.DoWithInterruptCurrent(OnGo, OnCompleted, OnTerminated,
                OnProgressChanged, OnStarted, OnEnded, OnAlert, OnStopped, userState);
        }

        #endregion Methods - Public

        #region Methods - Protected

        protected void Stop()
        {
        }

        #endregion Methods - Protected

        #region Methods - Override

        protected virtual void OnAlert(TaskAlertEventArgs e)
        {
        }

        protected virtual void OnCompleted(TaskCompletedEventArgs e)
        {
        }

        protected virtual void OnTerminated(TaskTerminatedEventArgs e)
        {
        }

        protected virtual void OnStopped(TaskStoppedEventArgs e)
        {
        }

        protected virtual void OnStarted(TaskStartedEventArgs e)
        {
        }

        protected virtual void OnProgressChanged(TaskProgressChangedEventArgs e)
        {
        }

        protected virtual void OnGo(TaskGoEventArgs e)
        {
        }

        protected virtual void OnEnded(TaskEndedEventArgs e)
        {
        }

        public override void Dispose()
        {
            Abort();
            Queue = null;
        }

        #endregion Methods - Override

        #endregion Methods
    }
}