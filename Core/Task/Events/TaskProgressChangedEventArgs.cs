using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public delegate void TaskProgressChangedEventHandler(object sender, TaskProgressChangedEventArgs e);

    [Serializable]
    public class TaskProgressChangedEventArgs : ProgressValueChangedEventArgs
    {
        #region Properties

        public Task Instance { get; internal set; }

        #endregion Properties

        #region Ctor

        public TaskProgressChangedEventArgs(int percent, object userState)
            : base(percent, userState)
        {
        }

        #endregion Ctor
    }
}