using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public delegate void MessageEventHandler(object sender, MsgEventArgs e);

    public class MsgEventArgs : EventArgs
    {
        #region Properties

        public eMessageGrade Grade { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual object Parameter { get; set; }
        public virtual object ReturnValue { get; set; }
        public virtual object Tag { get; set; }

        #endregion Properties

        #region Ctor

        public MsgEventArgs()
        {
            Grade = eMessageGrade.Infomation;
        }

        public MsgEventArgs(int msgID)
            : this()
        {
            this.ID = msgID;
        }

        public MsgEventArgs(string msgName)
            : this()
        {
            this.Name = msgName;
        }

        #endregion Ctor

        #region Methods

        public override string ToString()
        {
            var txt = Name;
            return txt == null ? string.Empty : txt;
        }

        #endregion Methods
    }

    public class MsgEventArgs<TIn> : MsgEventArgs
    {
        #region Properties

        public new virtual TIn Parameter
        {
            get { return (TIn)base.Parameter; }
            set { base.Parameter = value; }
        }

        public bool ToStringWithParameter { get; set; }

        #endregion Properties

        #region Ctor

        public MsgEventArgs()
        {
            ToStringWithParameter = true;
        }

        public MsgEventArgs(int msgID)
            : base(msgID)
        {
            ToStringWithParameter = true;
        }

        public MsgEventArgs(string msgName)
            : base(msgName)
        {
            ToStringWithParameter = true;
        }

        #endregion Ctor

        #region Methods

        public override string ToString()
        {
            if (ToStringWithParameter)
                return string.Format(base.ToString(), Parameter);
            else
                return base.ToString();
        }

        #endregion Methods
    }

    public class MsgEventArgs<TIn, TOut> : MsgEventArgs<TIn>
    {
        #region Properties

        public new virtual TOut ReturnValue
        {
            get { return (TOut)base.ReturnValue; }
            set { base.ReturnValue = value; }
        }

        #endregion Properties

        #region Ctor

        public MsgEventArgs()
        {
        }

        public MsgEventArgs(int msgID)
            : base(msgID)
        {
        }

        public MsgEventArgs(string msgName)
            : base(msgName)
        {
        }

        #endregion Ctor
    }

    public class KeyValueMsgEventArgs : MsgEventArgs<Dictionary<string, object>>
    {
        #region Ctor

        public KeyValueMsgEventArgs(int msgID)
            : base(msgID)
        {
            Parameter = new Dictionary<string, object>();
        }

        public KeyValueMsgEventArgs(string msgName)
            : base(msgName)
        {
            Parameter = new Dictionary<string, object>();
        }

        #endregion Ctor
    }

    public class KeyValueMsgEventArgs<TOut> : MsgEventArgs<Dictionary<string, object>, TOut>
    {
        #region Ctor

        public KeyValueMsgEventArgs(int msgID)
            : base(msgID)
        {
            Parameter = new Dictionary<string, object>();
        }

        public KeyValueMsgEventArgs(string msgName)
            : base(msgName)
        {
            Parameter = new Dictionary<string, object>();
        }

        #endregion Ctor
    }
}