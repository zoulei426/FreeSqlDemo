using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class ProgressValueChangedEventArgs : EventArgs
    {
        #region Properties

        public int Percent { get; set; }
        public object UserState { get; set; }

        #endregion Properties

        #region Ctor

        public ProgressValueChangedEventArgs(int percent, object userState)
        {
            Percent = percent;
            UserState = userState;
        }

        #endregion Ctor
    }
}