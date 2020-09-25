using Entity.Model;
using Prism.Mvvm;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Prism.Commands;

namespace FreeSqlDemo
{
    public class MainWindowViewModel : NotifyInfoCDObject
    {
        #region Properties

        public Account Account
        {
            get { return _Account; }
            set { _Account = value; NotifyPropertyChanged(() => Account); }
        }

        private Account _Account;

        public DelegateCommand ConfirmCommand { get; set; }

        #endregion Properties

        public MainWindowViewModel()
        {
            var resp = new AccountRepository();
            Account = resp.Get(new Guid("4c4b59b5-086b-4876-a6ce-0a4740cf7708")).ConvertTo<Account>();

            ConfirmCommand = new DelegateCommand(() =>
            {
                var a = Account.Error;
            });
        }
    }
}