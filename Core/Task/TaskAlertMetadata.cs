using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class TaskAlertMetadata : ComparableObject
    {
        #region Properties

        public int CountValue { get; set; }

        #endregion Properties

        #region Ctor

        public TaskAlertMetadata()
        {
            CountValue = 1;
        }

        #endregion Ctor
    }
}