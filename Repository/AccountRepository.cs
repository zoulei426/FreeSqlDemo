using Entity.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Repository
{
    public class AccountRepository
    {
        private static IFreeSql fsql = new FreeSql.FreeSqlBuilder()
            .UseConnectionString(FreeSql.DataType.Sqlite, "data source=test.db")
            .UseMonitorCommand(cmd => Trace.WriteLine($"线程：{cmd.CommandText}\r\n"))
            .UseAutoSyncStructure(true) //自动创建、迁移实体表结构
            .UseNoneCommandParameter(true)
            .Build();

        public void AddAccount(Account model)
        {
            var repo = fsql.GetRepository<Account>();
            repo.Insert(model);
        }

        public Account Get(Guid id)
        {
            var repo = fsql.GetRepository<Account>();

            //var query = repo.Select.AsQueryable();

            return (from account in repo.Select
                    where account.ID.Equals(id)
                    select account).First();
        }
    }
}