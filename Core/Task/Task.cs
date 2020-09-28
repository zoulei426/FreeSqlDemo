using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Core
{
    [Serializable]
    public class Task : CDObject, ITask, INotifyPropertyChanged
    {
        #region Properties

        public string Name
        {
            get { return _Name; }
            set { _Name = value; NotifyPropertyChanged("Name"); }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; NotifyPropertyChanged("Description"); }
        }

        private bool _ReportTime = false;

        public bool ReportTime
        {
            get { return _ReportTime = false; }
            set { _ReportTime = value; NotifyPropertyChanged("ReportTime"); }
        }

        public virtual TaskArgument Argument
        {
            get { return _Argument; }
            set { SetArgument(value); NotifyPropertyChanged("Argument"); }
        }

        public virtual bool AutoHandleException { get; set; }

        public Guid ID { get; set; }

        public bool IsBusy { get; private set; }
        public bool IsStopPending { get; private set; }
        public bool IsValidateMode { get; private set; }

        public virtual bool CanOpenResult
        {
            get { return _CanOpenResult; }
            protected set { _CanOpenResult = value; NotifyPropertyChanged("CanOpenResult"); }
        }

        #endregion Properties

        #region Fields

        private string _Name;
        private string _Description;
        private TaskArgument _Argument;
        private bool _CanOpenResult;
        private Dictionary<string, PropertyChangedHandlerAttribute> handlers;
        private int threadIDMain;
        private Stopwatch sw;

        #endregion Fields

        #region Events

        public event TaskGoEventHandler Go;

        public event TaskStartedEventHandler Started;

        public event TaskEndedEventHandler Ended;

        public event TaskCompletedEventHandler Completed;

        public event TaskStoppedEventHandler Stopped;

        public event TaskTerminatedEventHandler Terminated;

        public event TaskProgressChangedEventHandler ProgressChanged;

        public event TaskAlertEventHandler Alert;

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangedEventHandler ArgumentPropertyChanged;

        #endregion Events

        #region Ctor

        static Task()
        {
            //LanguageAttribute.AddLanguage(Properties.Resources.langChs_Task);
        }

        public Task()
        {
            Argument = new TaskArgument();
            handlers = PropertyChangedHandlerAttribute.Create(this.GetType());
            ID = Guid.NewGuid();
            Name = string.Empty;
            AutoHandleException = true;
            PropertyChanged += Task_PropertyChanged;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Static

        public static ITask Create(
            Action<TaskGoEventArgs> go,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> ended = null,
            Action<TaskCompletedEventArgs> comleted = null,
            Action<TaskStoppedEventArgs> stopped = null,
            Action<TaskTerminatedEventArgs> terminated = null)
        {
            return Create<Task>(go, alert, progressChanged, started, ended, comleted, stopped, terminated);
        }

        public static ITask Create<T>(
            Action<TaskGoEventArgs> go,
            Action<TaskAlertEventArgs> alert = null,
            Action<TaskProgressChangedEventArgs> progressChanged = null,
            Action<TaskStartedEventArgs> started = null,
            Action<TaskEndedEventArgs> Ended = null,
            Action<TaskCompletedEventArgs> comleted = null,
            Action<TaskStoppedEventArgs> stopped = null,
            Action<TaskTerminatedEventArgs> terminated = null) where T : Task, new()
        {
            var task = new T() { Argument = new TaskArgument() };
            task.Go += (s, e) => go(e);
            task.Alert += (s, e) => { if (alert != null) alert(e); };
            task.ProgressChanged += (s, e) => { if (progressChanged != null) progressChanged(e); };
            task.Started += (s, e) => { if (started != null) started(e); };
            task.Ended += (s, e) => { if (Ended != null) Ended(e); };
            task.Completed += (s, e) => { if (comleted != null) comleted(e); };
            task.Stopped += (s, e) => { if (stopped != null) stopped(e); };
            task.Terminated += (s, e) => { if (terminated != null) terminated(e); };

            return task;
        }

        #endregion Methods - Static

        #region Methods - Public

        public virtual void Start(bool validate = false)
        {
            lock (this)
                StartInner(validate);
        }

        private void StartInner(bool validate = false)
        {
            try
            {
                sw = Stopwatch.StartNew();
                threadIDMain = System.Threading.Thread.CurrentThread.ManagedThreadId;
                IsBusy = true;
                //IsStopPending = false;
                IsValidateMode = validate;
                CanOpenResult = false;

                ReportStarted();
                ReportGo();

                sw.Stop();
                ReportCompleted(sw);
            }
            catch (Exception ex)
            {
                IsStopPending = false;

                if (!(ex is TaskStopException))
                    ReportTerminated(ex);
                else
                    ReportStopped();
            }
            finally
            {
                ReportEnded();

                IsBusy = false;
                IsStopPending = false;
                IsValidateMode = false;
            }
        }

        public virtual void Stop()
        {
            IsStopPending = true;
        }

        public virtual void OpenResult()
        {
        }

        public virtual void Reset()
        {
            IsStopPending = false;
            CanOpenResult = false;
        }

        #endregion Methods - Public

        #region Methods - Override

        protected virtual void OnStarted()
        {
        }

        protected virtual void OnEnded()
        {
        }

        protected virtual void OnGo()
        {
        }

        protected virtual void OnValidate()
        {
        }

        protected virtual void OnStopped()
        {
        }

        protected virtual void OnCompleted(TaskCompletedEventArgs e)
        {
        }

        protected virtual void OnTerminate(Exception ex)
        {
        }

        protected virtual void OnProgressChanged(TaskProgressChangedEventArgs e)
        {
        }

        protected virtual void OnAlert(TaskAlertEventArgs e)
        {
        }

        protected virtual object OnGettingResult()
        {
            return Argument == null ? null : Argument.UserState;
        }

        protected virtual void OnArgumentPropertyChanged(PropertyChangedEventArgs e)
        {
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        #endregion Methods - Override

        #region Methods - Protected

        protected void NotifyPropertyChanged(string propertyName)
        {
            var evt = PropertyChanged;
            if (evt == null)
                return;

            evt(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyPropertyChanged<T>(System.Linq.Expressions.Expression<Func<T>> lambda)
        {
            LambdaPropertyNotifier.NotifyPropertyChanged(
                lambda, name => NotifyPropertyChanged(name));
        }

        public void ReportProgress(TaskProgressChangedEventArgs e)
        {
            e.Instance = this;
            e.Percent = e.Percent > 100 ? 100 : e.Percent;
            e.Percent = e.Percent < 0 ? 0 : e.Percent;

            TryStop();

            OnProgressChanged(e);
            if (ProgressChanged != null)
                ProgressChanged(this, e);
        }

        protected TimeSpan GetElapsedTime()
        {
            return sw.Elapsed;
        }

        #endregion Methods - Protected

        #region Methods - Report

        public void ReportAlert(TaskAlertEventArgs e)
        {
            TryStop();

            OnAlert(e);
            if (Alert != null)
                Alert(this, e);

            //if (e.Grade < eMessageGrade.Exception)
            //    return;

            //string msg = string.Format(LanguageAttribute.GetLanguage("lang670300"), e.UserState);
            //Tracker.WriteLine(new TrackerObject()
            //{
            //    Description = msg,
            //    EventID = 670300,
            //    Grade = e.Grade,
            //    Source = this.GetType().FullName
            //});
        }

        #endregion Methods - Report

        #region Methods - Events

        private void arg_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ReportArgumentPropertyChanged(e);
        }

        private void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(sender, e);

            if (handlers.ContainsKey(e.PropertyName))
                handlers[e.PropertyName].Method.Invoke(
                    this, new object[] { e.PropertyName, this.GetPropertyValue(e.PropertyName) });

            if (e.PropertyName == "Argument")
                ReportArgumentPropertyChanged(null);
        }

        #endregion Methods - Events

        #region Methods - Private

        private void TryStop()
        {
            if (IsStopPending && threadIDMain == System.Threading.Thread.CurrentThread.ManagedThreadId)
                //if (IsStopPending)
                throw new TaskStopException();
        }

        private void ReportArgumentPropertyChanged(PropertyChangedEventArgs e)
        {
            OnArgumentPropertyChanged(e);

            string name = e == null ? "Argument" : e.PropertyName;
            object value = e == null ? this.GetPropertyValue(name) : Argument.GetPropertyValue(name);

            if (handlers.ContainsKey(name))
                handlers[name].Method.Invoke(this, new object[] { name, value });

            if (ArgumentPropertyChanged != null)
                ArgumentPropertyChanged(this, e);
        }

        private void ReportStarted()
        {
            //if (ReportTime)
            //    ReportInfomation(string.Format(
            //        LanguageAttribute.GetLanguage("lang670301"), DateTime.Now));

            OnStarted();

            if (Started != null)
                Started(this, new TaskStartedEventArgs(this));
        }

        private void ReportEnded()
        {
            if (ReportTime)
                this.ReportInfomation(sw.Elapsed.ToString());

            OnEnded();

            if (Ended != null)
                Ended(this, new TaskEndedEventArgs());
        }

        private void ReportGo()
        {
            if (IsValidateMode)
                OnValidate();
            else
                OnGo();

            if (!IsValidateMode && Go != null)
                Go(this, new TaskGoEventArgs(this));
        }

        private void ReportCompleted(Stopwatch sw)
        {
            var e = new TaskCompletedEventArgs(this) { Result = OnGettingResult(), Argument = Argument, Elapsed = sw.Elapsed };

            OnCompleted(e);
            if (Completed != null)
                Completed(this, e);
        }

        private void ReportStopped()
        {
            OnStopped();
            if (Stopped != null)
                Stopped(this, new TaskStoppedEventArgs());
        }

        private void ReportTerminated(Exception ex)
        {
            ReportUnhandledException(ex);
            OnTerminate(ex);
            if (Terminated != null)
                Terminated(this, new TaskTerminatedEventArgs() { Exception = ex });
        }

        private void ReportUnhandledException(Exception ex)
        {
            string msg = ex.Message;

            TaskAlertEventArgs e = new TaskAlertEventArgs
            {
                Description = msg,
                Grade = eMessageGrade.Exception,
                UserState = ex,
            };

            OnAlert(e);
            if (Alert != null)
                Alert(this, e);

            if (!AutoHandleException)
                throw ex;
        }

        private void SetArgument(TaskArgument value)
        {
            UninstallArgument(_Argument);
            _Argument = value;
            InstallArgument(_Argument);
        }

        private void InstallArgument(TaskArgument arg)
        {
            if (arg == null)
                return;

            arg.PropertyChanged += arg_PropertyChanged;
        }

        private void UninstallArgument(TaskArgument arg)
        {
            if (arg == null)
                return;

            arg.PropertyChanged -= arg_PropertyChanged;
        }

        #endregion Methods - Private

        #region Methods - Serialize

        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        #endregion Methods - Serialize

        #endregion Methods
    }
}