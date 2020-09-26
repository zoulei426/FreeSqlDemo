using Core;
using FluentValidation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Entity.Models
{
    /// <summary>
    /// 账户类
    /// </summary>
    public class Account : NotifyInfoCDObject
    {
        public Guid ID
        {
            get { return _ID; }
            set { _ID = value; NotifyPropertyChanged(() => ID); }
        }

        private Guid _ID;

        [DisplayName("用户名")]
        [Description("用户名")]
        [Category("基础信息")]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; NotifyPropertyChanged(() => UserName); }
        }

        private string _UserName;

        [Description("密码")]
        public string Password
        {
            get { return _Password; }
            set { _Password = value; NotifyPropertyChanged(() => Password); }
        }

        private string _Password;

        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; NotifyPropertyChanged(() => CreateTime); }
        }

        private DateTime _CreateTime;

        public DateTime UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; NotifyPropertyChanged(() => UpdateTime); }
        }

        private DateTime _UpdateTime;

    }
}