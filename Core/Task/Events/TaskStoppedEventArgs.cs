using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public delegate void TaskStoppedEventHandler(object sender, TaskStoppedEventArgs e);

    [Serializable]
    public class TaskStoppedEventArgs : EventArgs
    {
        #region Ctor

        public TaskStoppedEventArgs()
        {
        }

        #endregion Ctor
    }
}