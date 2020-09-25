using Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Entity.Model
{
    public class Account : NotifyInfoCDObject
    {
        public Guid ID
        {
            get { return _ID; }
            set { _ID = value; NotifyPropertyChanged(() => ID); }
        }

        private Guid _ID;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; NotifyPropertyChanged(() => UserName); }
        }

        private string _UserName;

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

        protected override string Validate(string columnName)
        {
            if (validator == null)
            {
                validator = new AccountValidation();
            }
            //var a = new ValidationContext();
            var firstOrDefault = validator.Validate(this)
                .Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
            return firstOrDefault?.ErrorMessage;
        }

        private AccountValidation validator;
    }
}