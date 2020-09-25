using Entity.Database;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeSqlDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //InitializeData();
        }

        private void InitializeData()
        {
            var account = new Account();
            account.UserName = "Alice";
            account.Password = "123456";

            var resp = new AccountRepository();
            resp.AddAccount(account);
            //var account = resp.Get(new Guid("e3556d05-c1fe-4e7d-a258-651888d3c22d"));
        }
    }
}