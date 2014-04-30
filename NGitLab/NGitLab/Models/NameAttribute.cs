using System;

namespace NGitLab.Models
{
    /// <summary>
    /// Describes the name of a property.
    /// </summary>
    public class NameAttribute : Attribute
    {
        public readonly string Name;

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}