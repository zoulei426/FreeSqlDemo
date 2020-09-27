using System;

namespace Windows.Core
{
    /// <summary>
    /// Specifies that the text binding should be triggered at every change.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoUpdateTextAttribute : Attribute
    {
    }
}