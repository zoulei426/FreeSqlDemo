using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Windows.Control
{
    public class PropertyTrigger
    {
        #region Ctor

        public PropertyTrigger()
        {
        }

        #endregion Ctor

        #region Methods

        #region Methods - Public

        internal void PostPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
            OnPropertyValueChanged(pd, propertyName);
        }

        internal void PostPropertyValueInstalled(PropertyDescriptor pd, string propertyName)
        {
            OnPropertyValueInstalled(pd, propertyName);
        }

        public virtual void OnPropertyValueChanged(PropertyDescriptor pd, string propertyName)
        {
        }

        public virtual void OnPropertyValueInstalled(PropertyDescriptor pd, string propertyName)
        {
        }

        #endregion Methods - Public

        #region Methods - Protected

        protected void RaiseAlert(PropertyDescriptor pd, eMessageGrade grade, string description)
        {
            pd.Designer.Dispatcher.Invoke(new Action(() =>
            {
                pd.Grade = grade;
                pd.DescriptionState = description;

                //switch (grade)
                //{
                //    case eMessageGrade.Warn:
                //        pd.ImageState = BitmapFrame.Create(new Uri(
                //            "pack://application:,,,/YuLinTu.Resources;component/Images/16/Warning.png"));
                //        break;

                //    case eMessageGrade.Error:
                //    case eMessageGrade.Exception:
                //        pd.ImageState = BitmapFrame.Create(new Uri(
                //            "pack://application:,,,/YuLinTu.Resources;component/Images/16/Error.png"));
                //        break;

                //    case eMessageGrade.Infomation:
                //    default:
                //        pd.ImageState = description.IsNullOrBlank() ?
                //            BitmapFrame.Create(new Uri(
                //                "pack://application:,,,/YuLinTu.Resources;component/Images/16/Success.png")) :
                //            BitmapFrame.Create(new Uri(
                //                "pack://application:,,,/YuLinTu.Resources;component/Images/16/Information.png"));
                //        break;
                //}

                pd.PropertyGrid.RaiseAlert(new PropertyGridAlertEventArgs()
                {
                    PropertyDescriptor = pd,
                    Grade = grade,
                    Description = description
                });
            }));
        }

        #endregion Methods - Protected

        #endregion Methods
    }
}