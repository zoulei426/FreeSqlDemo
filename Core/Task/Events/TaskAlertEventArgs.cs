using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Core
{
    public delegate void TaskAlertEventHandler(object sender, TaskAlertEventArgs e);

    [Serializable]
    public class TaskAlertEventArgs : EventArgs, INotifyPropertyChanged
    {
        #region Properties

        public bool IsCancel { get; set; }
        public object UserState { get; set; }
        public DateTime DateTime { get; set; }
        public TaskAlertMetadata Metadata { get; set; }

        private eMessageGrade _Grade;

        public eMessageGrade Grade
        {
            get { return _Grade; }
            set { _Grade = value; NotifyPropertyChanged("Grade"); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; NotifyPropertyChanged("Description"); }
        }

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events

        #region Ctor

        public TaskAlertEventArgs()
        {
            Grade = eMessageGrade.Infomation;
            IsCancel = false;
            DateTime = DateTime.Now;
        }

        public TaskAlertEventArgs(object userState, string description)
            : this()
        {
            UserState = userState;
            Description = description;
        }

        public TaskAlertEventArgs(eMessageGrade grade, object userState, string description)
            : this(userState, description)
        {
            Grade = grade;
        }

        #endregion Ctor

        #region Methods

        public override string ToString()
        {
            return string.Format("{0}, {1}", Grade, Description);
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt == null)
                return;

            evt(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Methods
    }
}