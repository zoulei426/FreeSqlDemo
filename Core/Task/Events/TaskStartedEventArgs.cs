using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public delegate void TaskStartedEventHandler(object sender, TaskStartedEventArgs e);

    [Serializable]
    public class TaskStartedEventArgs : EventArgs
    {
        #region Properties

        public Task Instance { get; private set; }

        #endregion Properties

        #region Ctor

        public TaskStartedEventArgs(Task task)
        {
            Instance = task;
        }

        #endregion Ctor
    }
}