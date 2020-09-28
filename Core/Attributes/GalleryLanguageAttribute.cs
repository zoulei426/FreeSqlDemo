using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.All)]
    public class GalleryLanguageAttribute : Attribute
    {
        #region Fields

        private string name;

        #endregion Fields

        #region Properties

        public bool IsLanguageName { get; set; }

        public string Name
        {
            get { return GetName(); }
        }

        #endregion Properties

        #region Ctor

        public GalleryLanguageAttribute(string name)
        {
            this.name = name;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Helper

        private string GetName()
        {
            if (!IsLanguageName)
                return name;

            return LanguageAttribute.GetLanguage(name);
        }

        #endregion Methods - Helper

        #endregion Methods
    }
}