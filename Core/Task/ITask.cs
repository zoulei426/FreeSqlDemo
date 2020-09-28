using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core
{
    public interface ITask : ICloneable, IDisposable
    {
        #region Properties

        TaskArgument Argument { get; set; }
        bool IsBusy { get; }
        bool IsValidateMode { get; }
        Guid ID { get; }

        string Name { get; set; }
        string Description { get; set; }

        #endregion Properties

        #region Events

        event TaskGoEventHandler Go;

        event TaskStartedEventHandler Started;

        event TaskEndedEventHandler Ended;

        event TaskCompletedEventHandler Completed;

        event TaskStoppedEventHandler Stopped;

        event TaskTerminatedEventHandler Terminated;

        event TaskProgressChangedEventHandler ProgressChanged;

        event TaskAlertEventHandler Alert;

        event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler ArgumentPropertyChanged;

        #endregion Events

        #region Methods

        void Start(bool validate = false);

        void Stop();

        void Reset();

        void ReportProgress(TaskProgressChangedEventArgs e);

        void ReportAlert(TaskAlertEventArgs e);

        #endregion Methods
    }
}