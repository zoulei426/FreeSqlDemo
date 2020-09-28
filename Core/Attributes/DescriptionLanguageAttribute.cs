using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    [AttributeUsage(AttributeTargets.All)]
    public class DescriptionLanguageAttribute : Attribute
    {
        #region Fields

        private string description;

        #endregion Fields

        #region Properties

        public bool IsLanguageName { get; set; }

        public string Description
        {
            get { return GetDescription(); }
        }

        #endregion Properties

        #region Ctor

        public DescriptionLanguageAttribute(string description)
        {
            this.description = description;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Helper

        private string GetDescription()
        {
            if (!IsLanguageName)
                return description;

            return LanguageAttribute.GetLanguage(description);
        }

        #endregion Methods - Helper

        #endregion Methods
    }
}