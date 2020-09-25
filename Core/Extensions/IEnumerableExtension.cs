using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core
{
    public static class IEnumerableExtension
    {
        #region Methods

        /// <summary>
        /// ToList 的安全版本，如果发生异常并不会导致程序崩溃，而是返回null。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<T> TryToList<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return null;

            try { return source.ToList(); }
            catch { return null; }
        }

        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            if (source == null)
                return true;

            bool has = source.GetEnumerator().MoveNext();
            if (!has)
                return true;

            return false;
        }

        public static IList Clone(this IList source)
        {
            if (source == null)
                return null;

            MethodInfo mi = source.GetType().GetMethod("Clone");
            if (mi != null)
                return (IList)mi.Invoke(source, null);

            IList list = Activator.CreateInstance(source.GetType()) as IList;
            MethodInfo addMethod = list.GetType().GetMethod("Add");

            foreach (object item in source)
                addMethod.Invoke(list, new object[] { CDObject.TryClone(item) });

            return list;
        }

        public static void Sort<T>(this List<T> source, string propertyPath, eOrder order)
        {
            source.Sort((a, b) =>
            {
                var valA = a.GetPropertyValue(propertyPath, true, false) as IComparable;
                var valB = b.GetPropertyValue(propertyPath, true, false) as IComparable;

                var val = 0;

                if (valA != null && valB != null)
                    val = valA.CompareTo(valB);
                else if (valA == null && valB != null)
                    val = -1;
                else if (valA != null && valB == null)
                    val = 1;
                else
                    val = 0;

                return order == eOrder.Ascending ? val : -val;
            });
        }

        public static List<T> ToRandomSortList<T>(this List<T> list)
        {
            Random random = new Random();
            List<T> newList = new List<T>();
            foreach (T item in list)
                newList.Insert(random.Next(newList.Count + 1), item);

            return newList;
        }

        #endregion Methods
    }
}