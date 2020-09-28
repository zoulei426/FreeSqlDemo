using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class TaskDelegateMetadata
    {
        #region Properties

        public Action<TaskGoEventArgs> Go { get; set; }
        public Action<TaskAlertEventArgs> Alert { get; set; }
        public Action<TaskProgressChangedEventArgs> ProgressChanged { get; set; }
        public Action<TaskStartedEventArgs> Started { get; set; }
        public Action<TaskEndedEventArgs> Ended { get; set; }
        public Action<TaskCompletedEventArgs> Completed { get; set; }
        public Action<TaskStoppedEventArgs> Stopped { get; set; }
        public Action<TaskTerminatedEventArgs> Terminated { get; set; }
        public object UserState { get; set; }

        #endregion Properties
    }
}