using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Windows.Control
{
    public class PropertyDescriptorAttribute : Attribute
    {
        #region Properties

        public Type Builder { get; set; }
        public Type Trigger { get; set; }
        public Type Converter { get; set; }
        public object ConverterParameter { get; set; }
        public string UriImage16 { get; set; }

        public string Class { get; set; }

        public string Gallery { get; set; }
        public string GalleryImage16 { get; set; }

        public string Catalog { get; set; }
        public string CatalogImage16 { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }
        public int ColumnSpan { get; set; }
        public double Height { get; set; }

        public bool Editable { get; set; }

        public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

        #endregion Properties

        #region Ctor

        public PropertyDescriptorAttribute()
        {
            Class = string.Empty;
            UpdateSourceTrigger = UpdateSourceTrigger.LostFocus;
            Gallery = string.Empty;
            Catalog = string.Empty;
            Editable = true;
            Column = 2;
            ColumnSpan = 1;
        }

        #endregion Ctor

        #region Methods

        public PropertyDescriptorBuilder CreateBuilder()
        {
            if (Builder == null)
                return null;

            return Activator.CreateInstance(Builder) as PropertyDescriptorBuilder;
        }

        public PropertyTrigger CreateTrigger()
        {
            if (Trigger == null)
                return null;

            return Activator.CreateInstance(Trigger) as PropertyTrigger;
        }

        #endregion Methods
    }
}