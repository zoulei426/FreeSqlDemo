using Entity.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 账户校验类
    /// </summary>
    public class AccountValidation : AbstractValidator<Account>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public AccountValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空")
                .Length(2, 30).WithMessage("用户名长度在2-30个字符之间");
        }
    }
}