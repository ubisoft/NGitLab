using System;

namespace NGitLab.Models
{
    /// <summary>
    /// Decorate your model classes to skip the fields for which
    /// the value is not specified. This is useful in particular for the updates.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class SkipNullFieldsAttribute : Attribute
    {
        public bool SkipNullFields { get; set; }
    }
}
