using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Core
{
    public static class TaskExtension
    {
        #region Methods

        public static void StartAsync(this ITask source, bool validate = false)
        {
            ThreadWorkstaion.Start(new Action(() => { source.Start(validate); }));
        }

        public static void StartAsync(this ITask source, ApartmentState state, bool validate = false)
        {
            ThreadWorkstaion.Start(new Action(() => { source.Start(validate); }), state);
        }

        public static void ReportProgress(this ITask source, int percent, object userState = null)
        {
            TaskProgressChangedEventArgs e = new TaskProgressChangedEventArgs(percent, userState);
            source.ReportProgress(e);
        }

        public static bool ReportInfomation(this ITask source, string description, string catalog = null, TaskAlertMetadata meta = null)
        {
            TaskAlertEventArgs e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Infomation,
                UserState = catalog.IsNullOrBlank() ? null : new TaskAlertUserStateCatalog() { Catalog = catalog },
                Metadata = meta,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportError(this ITask source, string description, string catalog = null, TaskAlertMetadata meta = null)
        {
            TaskAlertEventArgs e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Error,
                UserState = catalog.IsNullOrBlank() ? null : new TaskAlertUserStateCatalog() { Catalog = catalog },
                Metadata = meta,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportWarn(this ITask source, string description, string catalog = null, TaskAlertMetadata meta = null)
        {
            TaskAlertEventArgs e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Warn,
                UserState = catalog.IsNullOrBlank() ? null : new TaskAlertUserStateCatalog() { Catalog = catalog },
                Metadata = meta,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportException(this ITask source, Exception ex)
        {
            string msg = ex.Message;
            return source.ReportException(ex, msg);
        }

        public static bool ReportException(this ITask source, Exception ex, string description)
        {
            TaskAlertEventArgs e = new TaskAlertEventArgs
            {
                Description = description,
                Grade = eMessageGrade.Exception,
                UserState = ex,
            };

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportAlert(this ITask source, object userState, string description)
        {
            TaskAlertEventArgs e = new TaskAlertEventArgs(userState, description);

            source.ReportAlert(e);

            return e.IsCancel;
        }

        public static bool ReportAlert(this ITask source, eMessageGrade grade, object userState, string description)
        {
            TaskAlertEventArgs e = new TaskAlertEventArgs(grade, userState, description);
            source.ReportAlert(e);

            return e.IsCancel;
        }

        #endregion Methods
    }
}