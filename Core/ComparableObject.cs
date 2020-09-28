using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core
{
    [Serializable]
    public class ComparableObject : CDObject, IComparable
    {
        #region Properties

        [DataMember]
        public virtual string Name { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        public virtual int CompareTo(object obj)
        {
            ComparableObject objName = obj as ComparableObject;
            if (objName == null)
                return 0;

            return Name.CompareTo(objName.Name);
        }

        public override void Dispose()
        {
            base.Dispose();

            Name = null;
        }

        #endregion Methods
    }
}