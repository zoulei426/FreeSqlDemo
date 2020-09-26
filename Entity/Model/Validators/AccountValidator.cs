using Entity.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 账户验证器
    /// </summary>
    public class AccountValidator : AbstractValidator<Account>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public AccountValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空")
                .Length(2, 30).WithMessage("用户名长度在2-30个字符之间");
        }
    }
}