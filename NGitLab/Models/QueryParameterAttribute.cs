using System;

namespace NGitLab.Models
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class QueryParameterAttribute : Attribute
    {
        public QueryParameterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
