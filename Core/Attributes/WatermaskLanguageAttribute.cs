using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.All)]
    public class WatermaskLanguageAttribute : Attribute
    {
        #region Fields

        private string name;

        #endregion Fields

        #region Properties

        public bool IsLanguageName { get; set; }

        public string Name
        {
            get { return GetDescription(); }
        }

        #endregion Properties

        #region Ctor

        public WatermaskLanguageAttribute(string name)
        {
            this.name = name;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Helper

        private string GetDescription()
        {
            if (!IsLanguageName)
                return name;

            return LanguageAttribute.GetLanguage(name);
        }

        #endregion Methods - Helper

        #endregion Methods
    }
}