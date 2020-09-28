using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.All)]
    public class ImageAttribute : Attribute
    {
        #region Properties

        public string ImageUri { get; set; }

        #endregion Properties

        #region Ctor

        public ImageAttribute(string uri)
        {
            this.ImageUri = uri;
        }

        #endregion Ctor
    }
}