using System.Collections.Generic;

namespace Windows.Core
{
    /// <summary>
    /// Defines functionality to build the model for a <see cref="PropertyGrid" />.
    /// </summary>
    public interface IPropertyGridOperator
    {
        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="isEnumerable">if set to <c>true</c> enumerable types instances will use the enumerated objects instead of the instance itself.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The tabs.
        /// </returns>
        IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyGridOptions options);
    }
}