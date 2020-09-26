using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core
{
    [Serializable]
    public class NotifyInfoCDObject : NotifyCDObject, IDataErrorInfo
    {
        #region Fields

        /// <summary>
        /// 验证错误集合
        /// </summary>
        private Dictionary<string, string> _DataErrors = new Dictionary<string, string>();

        /// <summary>
        /// 验证器
        /// </summary>
        private object validator;

        private MethodInfo validateMI;

        #endregion Fields

        #region Properties

        public virtual string Error { get { return _Error; } }
        private string _Error;
        public virtual string this[string columnName] { get { return Validate(columnName); } }

        #endregion Properties

        #region Ctor

        public NotifyInfoCDObject()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Virtual

        protected virtual string Validate(string columnName)
        {
            try
            {
                if (validator == null || validateMI == null)
                {
                    var type = GetType();
                    var className = type.Name;
                    var assembly = type.Assembly;
                    var validatorCls = assembly.GetTypes().FirstOrDefault(t => t.Name.Equals(className + "Validator"));
                    validator = Activator.CreateInstance(validatorCls, true);//根据类型创建实例
                    validateMI = validatorCls.GetMethods().FirstOrDefault(t => t.Name.Equals("Validate"));
                }
               
               
                var result = validateMI.Invoke(validator, new object[] { this }) as FluentValidation.Results.ValidationResult;

                var firstOrDefault = result.Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                return firstOrDefault?.ErrorMessage;


                //ValidationContext vc = new ValidationContext(this, null, null);
                //vc.MemberName = columnName;
                //var res = new List<ValidationResult>();
                //var result = Validator.TryValidateProperty(this.GetType().GetProperty(columnName).GetValue(this, null), vc, res);
                //if (res.Count > 0)
                //{
                //    AddDic(_DataErrors, vc.MemberName);
                //    return string.Join(Environment.NewLine, res.Select(r => r.ErrorMessage).ToArray());
                //}
                //RemoveDic(_DataErrors, vc.MemberName);
                //return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion Methods - Virtual

        #region Methods - private

        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="dicKey"></param>
        private void RemoveDic(Dictionary<String, String> dics, String dicKey)
        {
            dics.Remove(dicKey);
        }

        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="dics"></param>
        /// <param name="dicKey"></param>
        private void AddDic(Dictionary<String, String> dics, String dicKey)
        {
            if (!dics.ContainsKey(dicKey)) dics.Add(dicKey, "");
        }

        #endregion Methods - private

        #endregion Methods
    }
}