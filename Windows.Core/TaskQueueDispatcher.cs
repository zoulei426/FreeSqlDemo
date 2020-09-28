using Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace Windows.Core
{
    public sealed class TaskQueueDispatcher : TaskQueue
    {
        #region Properties

        public Dispatcher Dispatcher { get; set; }

        #endregion Properties

        #region Ctor

        public TaskQueueDispatcher()
            : this(System.Windows.Application.Current.Dispatcher)
        {
        }

        public TaskQueueDispatcher(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Override

        protected override void OnTerminated(TaskDelegateMetadata meta, TaskTerminatedEventArgs terminated)
        {
            if (meta.Terminated != null)
                Dispatcher.Invoke(new Action(() => meta.Terminated(terminated)));
        }

        protected override void OnStopped(TaskDelegateMetadata meta, TaskStoppedEventArgs stopped)
        {
            if (meta.Stopped != null)
                Dispatcher.Invoke(new Action(() => meta.Stopped(stopped)));
        }

        protected override void OnCompleted(TaskDelegateMetadata meta, TaskCompletedEventArgs completed)
        {
            if (meta.Completed != null)
                Dispatcher.Invoke(new Action(() => meta.Completed(completed)));
        }

        protected override void OnEnded(TaskDelegateMetadata meta, TaskEndedEventArgs ended)
        {
            if (meta.Ended != null)
                Dispatcher.Invoke(new Action(() => meta.Ended(ended)));
        }

        protected override void OnStarted(TaskDelegateMetadata meta, TaskStartedEventArgs started)
        {
            if (meta.Started != null)
                Dispatcher.Invoke(new Action(() => meta.Started(started)));
        }

        protected override void OnProgressChanged(TaskDelegateMetadata meta, TaskProgressChangedEventArgs progressChanged)
        {
            if (meta.ProgressChanged != null)
                Dispatcher.Invoke(new Action(() => meta.ProgressChanged(progressChanged)));
        }

        protected override void OnAlert(TaskDelegateMetadata meta, TaskAlertEventArgs alert)
        {
            if (meta.Alert != null)
                Dispatcher.Invoke(new Action(() => meta.Alert(alert)));
        }

        protected override void OnGo(TaskDelegateMetadata meta, TaskGoEventArgs go)
        {
            if (meta.Go != null)
                meta.Go(go);
        }

        #endregion Methods - Override

        #endregion Methods
    }
}