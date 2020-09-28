using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Xml.Linq;

namespace Core
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class LanguageAttribute : Attribute
    {
        #region Properties

        public string Name
        {
            get { return name; }
        }

        public string Language
        {
            get { return IsLanguageName ? GetLanguage(name) : name; }
        }

        public bool IsLanguageName { get; set; }

        #endregion Properties

        #region Fields

        #region Fields - Infomation

        private string name;

        #endregion Fields - Infomation

        #region Fields - Static

        private static readonly object objSync;
        private static Dictionary<string, string> listLanguage;

        #endregion Fields - Static

        #endregion Fields

        #region Ctor

        static LanguageAttribute()
        {
            objSync = new object();

            CreateLanguageList();
        }

        public LanguageAttribute(string name)
        {
            this.name = name;
            IsLanguageName = true;
        }

        #endregion Ctor

        #region Methods

        #region Methods - Static

        public static void AddLanguage(string stringXmlLanguage)
        {
            lock (objSync)
            {
                try
                {
                    XDocument doc = XDocument.Load(new StringReader(stringXmlLanguage));
                    XElement element = doc.Root;

                    foreach (XNode node in element.Nodes())
                    {
                        XElement el = node as XElement;
                        if (el == null || listLanguage.ContainsKey(el.Name.LocalName))
                            continue;

                        listLanguage.Add(el.Name.LocalName, el.Value);
                    }
                }
                catch { return; }
            }
        }

        public static void SetLanguage(string stringXmlLanguage)
        {
            lock (objSync)
            {
                try
                {
                    XDocument doc = XDocument.Load(new StringReader(stringXmlLanguage));
                    XElement element = doc.Root;

                    foreach (XNode node in element.Nodes())
                    {
                        XElement el = node as XElement;
                        if (el == null)
                            continue;

                        listLanguage[el.Name.LocalName] = el.Value;
                    }
                }
                catch { return; }
            }
        }

        public static string GetLanguage(string name)
        {
            string lang = string.Empty;

            if (!listLanguage.ContainsKey(name))
                lang = string.Format((string)listLanguage["ex22222"], name);
            else
                lang = (string)listLanguage[name];

            return lang;
        }

        private static void CreateLanguageList()
        {
            listLanguage = new Dictionary<string, string>();
            listLanguage.Add("ex22222", "{0}?");
        }

        #endregion Methods - Static

        #endregion Methods
    }
}