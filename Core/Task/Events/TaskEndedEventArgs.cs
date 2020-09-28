using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public delegate void TaskEndedEventHandler(object sender, TaskEndedEventArgs e);

    [Serializable]
    public class TaskEndedEventArgs : EventArgs
    {
        #region Ctor

        public TaskEndedEventArgs()
        {
        }

        #endregion Ctor
    }
}