using System;

namespace NGitLab.Models;

/// <summary>
/// Identifies a field or property that represents a REST API Query Parameter
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class QueryParameterAttribute : Attribute
{
    public QueryParameterAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
