﻿using Entity.Models;
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
            var id = new Guid("5f6e55b3-eff9-b16c-0097-2644033ba3f5");
            var resp = new AccountRepository();
            Account = resp.Get(id)?.ConvertTo<Account>();

            if (Account == null)
            {
                var account = new Account();
                account.ID = id;
                account.UserName = "Alice";
                account.Password = "123456";

                //var resp = new AccountRepository();
                resp.AddAccount(account.ConvertTo<Entity.Database.Account>());

                Account = resp.Get(id)?.ConvertTo<Account>();
            }

            ConfirmCommand = new DelegateCommand(() =>
            {
                var a = Account.Error;
            });
        }
    }
}