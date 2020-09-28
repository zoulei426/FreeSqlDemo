using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core
{
    /// <summary>
    /// Type扩展
    /// </summary>
    public static class TypeExtension
    {
        #region Methods

        #region Methods - Traversal

        public static void TraversalMethodsInfo(this Type source, Func<MethodInfo, bool> method)
        {
            if (method == null || source == null)
                return;

            MethodInfo[] list = source.GetMethods();

            foreach (MethodInfo fi in list)
            {
                if (!method(fi))
                    return;
            }
        }

        public static void TraversalPropertiesInfo(this Type source, Func<PropertyInfo, bool> method, BindingFlags? flags = null)
        {
            if (method == null || source == null)
                return;

            PropertyInfo[] listPropertyInfo = flags == null ?
                source.GetProperties() : source.GetProperties(flags.Value);

            foreach (PropertyInfo pi in listPropertyInfo)
                if (!method(pi))
                    return;
        }

        public static void TraversalBaseType(this Type source, Func<Type, bool> method)
        {
            if (method == null || source == null)
                return;

            do
            {
                if (!method(source))
                    break;

                source = source.BaseType;
            } while (source != null);
        }

        #endregion Methods - Traversal

        #region Methods - Type

        public static bool IsEnumerable(this Type source)
        {
            return IsKindOf(source, typeof(IEnumerable));
        }

        public static bool IsKindOf(this Type source, Type target)
        {
            if (source == null)
                return false;

            if (source == target)
                return true;

            if (IsKindOf(source.BaseType, target))
                return true;

            foreach (Type tpInterface in source.GetInterfaces())
                if (IsKindOf(tpInterface, target))
                    return true;

            return false;
        }

        public static object GetDefaultValue(this Type source)
        {
            if (source.IsEnum)
                return Enum.ToObject(source, Enum.GetValues(source).GetValue(0));
            else if (source.IsValueType)
                return Activator.CreateInstance(source);

            return null;
        }

        public static object GetDefaultValueNoNull(this Type source)
        {
            object value = null;
            if (source == typeof(string))
                value = string.Empty;
            else if (source.IsNullableGeneric())
                value = Activator.CreateInstance(source.GetGenericTypeInNullable());
            else
                value = Activator.CreateInstance(source);

            return value;
        }

        public static bool IsValueTypeOrString(this Type source)
        {
            return source.IsValueType ||
                   source.IsEnum ||
                   source.IsKindOf(typeof(string));
        }

        public static bool IsNullable(this Type source)
        {
            return !source.IsValueType || source.IsNullableGeneric();
        }

        public static bool IsNullableGeneric(this Type source)
        {
            return source.FullName.StartsWith("System.Nullable`1") && source.IsGenericType;
        }

        public static Type GetGenericTypeInNullable(this Type source)
        {
            if (!source.IsNullableGeneric())
                return null;

            return source.GetGenericArguments()[0];
        }

        public static string GetAssemblyTypeName(this Type source)
        {
            return string.Format("{0}, {1}", source.FullName, source.Assembly.FullName);
        }

        public static string GetTypeNameWithoutQualified(this Type source)
        {
            if (!source.IsGenericType)
                return string.Format("{0}, {1}", source.FullName, source.Assembly.GetName().Name);

            var args = source.GetGenericArguments();

            return string.Format("{0}.{1}[{2}], {3}",
                source.Namespace,
                source.Name,
                BuildTypeName(args),
                source.Assembly.GetName().Name);
        }

        private static string BuildTypeName(Type[] args)
        {
            StringBuilder b = new StringBuilder(string.Format("[{0}]", args[0].GetTypeNameWithoutQualified()));
            for (int i = 1; i < args.Length; i++)
                b.AppendFormat(", [{0}]", args[i].GetTypeNameWithoutQualified());

            return b.ToString();
        }

        private static object CreateInstance(this Type source, Dictionary<string, object> values)
        {
            object obj = Activator.CreateInstance(source);

            foreach (var pair in values)
                obj.SetPropertyValue(pair.Key, pair.Value);

            return obj;
        }

        #endregion Methods - Type

        #endregion Methods

        /// <summary>
        /// The generic sequence type.
        /// </summary>
        private static readonly Type GenericEnumerableType = typeof(IEnumerable<>);

        /// <summary>
        /// Finds the biggest common type of items in the list.
        /// </summary>
        /// <param name="items">The list.</param>
        /// <returns>
        /// The biggest common type.
        /// </returns>
        public static Type FindBiggestCommonType(this IEnumerable items)
        {
            if (items == null)
            {
                return null;
            }

            Type type = null;
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                var itemType = item.GetType();
                if (type == null)
                {
                    type = itemType;
                    continue;
                }

                while (type != null && type.BaseType != null && !type.IsAssignableFrom(itemType))
                {
                    type = type.BaseType;
                }
            }

            if (type == null && items is IList)
            {
                // it is safe to enumerate items again here
                type = items.AsQueryable().ElementType;
            }

            return type;
        }

        /// <summary>
        /// Gets the underlying enum type of the specified type, if the specified type is a nullable type.
        /// </summary>
        /// <param name="propertyType">The type.</param>
        /// <returns>
        /// The type of the underlying enum.
        /// </returns>
        public static Type GetEnumType(this Type propertyType)
        {
            if (propertyType == null)
            {
                return null;
            }

            var ult = Nullable.GetUnderlyingType(propertyType);
            if (ult != null)
            {
                propertyType = ult;
            }

            if (typeof(Enum).IsAssignableFrom(propertyType))
            {
                return propertyType;
            }

            return null;
        }

        /// <summary>
        /// Gets the type of the items in the specified enumeration.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        /// The type of the items.
        /// </returns>
        public static Type GetItemType(this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return null;
            }

            try
            {
                // If enumerable is queryable, get the type from the expression tree of the queryable is executed
                return enumerable.AsQueryable().ElementType;
            }
            catch (ArgumentException)
            {
                // otherwise try to get the type from the IEnumerable<> interface
                var ifs = enumerable.GetType().GetInterfaces();
                var i = ifs.FirstOrDefault(it => it.IsGenericType && it.GetGenericTypeDefinition() == GenericEnumerableType);
                return i?.GetGenericArguments()[0];
            }
        }

        /// <summary>
        /// Gets the item type from a list type.
        /// </summary>
        /// <param name="listType">The list type.</param>
        /// <returns>
        /// The <see cref="Type" /> of the elements.
        /// </returns>
        public static Type GetListElementType(this Type listType)
        {
            // http://stackoverflow.com/questions/1043755/c-generic-list-t-how-to-get-the-type-of-t
            foreach (var interfaceType in listType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    var args = interfaceType.GetGenericArguments();
                    if (args.Length > 0)
                    {
                        return args[0];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the first type is assignable from the specified second type.
        /// </summary>
        /// <param name="firstType">Type of the first type.</param>
        /// <param name="secondType">The type of the second type.</param>
        /// <returns>
        /// True if it is assignable.
        /// </returns>
        public static bool Is(this Type firstType, Type secondType)
        {
            if (firstType.IsGenericType && secondType == firstType.GetGenericTypeDefinition())
            {
                return true;
            }

            // checking generic interfaces
            foreach (var @interface in firstType.GetInterfaces())
            {
                if (@interface.IsGenericType)
                {
                    if (secondType == @interface.GetGenericTypeDefinition())
                    {
                        return true;
                    }
                }

                if (secondType == @interface)
                {
                    return true;
                }
            }

            var ult = Nullable.GetUnderlyingType(firstType);
            if (ult != null)
            {
                if (secondType.IsAssignableFrom(ult))
                {
                    return true;
                }
            }

            if (secondType.IsAssignableFrom(firstType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets inner generic type of an IList&gt;IList&lt;
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// The <see cref="Type" />.
        /// </returns>
        public static Type GetInnerMostGenericType(this IList list)
        {
            var genericArguments = list.GetType().GetGenericArguments();
            var innerType = genericArguments.Length > 0 ? genericArguments[0] : null;

            if (innerType != null && innerType.IsGenericType)
            {
                var innerGenericArguments = innerType.GetGenericArguments();
                var innerMostType = genericArguments.Length > 0 ? innerGenericArguments[0] : null;
                return innerMostType;
            }

            return innerType;
        }

        /// <summary>
        /// Gets the type of the inner list of a IList&gt;IList&lt;
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>
        /// The type of the inner list. Return <c>null</c> if only interface type can be retrieved.
        /// </returns>
        public static Type GetInnerTypeOfList(this IList list)
        {
            var innerType = TypeExtension.GetInnerMostGenericType(list);
            if (innerType != null && innerType.IsInterface)
            {
                if (list.Count > 0)
                {
                    var row = list[0] as IList;
                    if (row != null && row.Count > 0)
                    {
                        // Get the type from the [0][0]. The assumption is all the elements in the ItemsSource are of the same type.
                        innerType = row[0].GetType();
                    }
                }
                else
                {
                    innerType = null;
                }
            }

            return innerType;
        }

        /// <summary>
        /// Determines whether the type is IList{IList}.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the type is IList{IList}; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIListIList(this Type type)
        {
            if (!typeof(IList).IsAssignableFrom(type))
            {
                return false;
            }

            var listElementType = GetListElementType(type);
            if (listElementType == null)
            {
                return false;
            }

            if (listElementType == typeof(string))
            {
                return false;
            }

            if (listElementType.IsGenericType && listElementType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return true;
            }

            return GetListElementType(listElementType) != null;
        }
    }
}