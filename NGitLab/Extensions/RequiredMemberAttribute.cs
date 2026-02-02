#if NETFRAMEWORK || NETSTANDARD2_0
#nullable enable
#pragma warning disable

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
 
    [DebuggerNonUserCode]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    internal sealed class RequiredMemberAttribute : Attribute { }
}

#pragma warning restore
#nullable restore
#endif //NETFRAMEWORK
