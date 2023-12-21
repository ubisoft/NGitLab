using System.ComponentModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NGitLab.Mock")]
[assembly: InternalsVisibleTo("NGitLab.Tests")]

// The following is to support the property 'init' keyword.
// See https://blog.ndepend.com/using-c9-record-and-init-property-in-your-net-framework-4-x-net-standard-and-net-core-projects/
namespace System.Runtime.CompilerServices;

/// <summary>
/// Reserved to be used by the compiler for tracking metadata.
/// This class should not be used by developers in source code.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit
{
}
