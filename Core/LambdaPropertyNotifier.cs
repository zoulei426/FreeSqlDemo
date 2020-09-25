using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core
{
    public class LambdaPropertyNotifier
    {
        #region Methods

        public static void NotifyPropertyChanged<T>(Expression<Func<T>> lambda, Action<string> notifyCallback)
        {
            notifyCallback?.Invoke((lambda.Body as MemberExpression).Member.Name);
        }

        #endregion Methods
    }
}