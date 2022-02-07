using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class AsyncApiValidation
    {
        [Test]
        public void ValidateAsyncMethodSignature()
        {
            var interfaces = typeof(IGitLabClient).Assembly.GetTypes().Where(t => t.IsInterface && t.IsPublic && t != typeof(IHttpRequestor));
            foreach (var iface in interfaces)
            {
                foreach (var method in iface.GetMethods())
                {
                    if (typeof(Task).IsAssignableFrom(method.ReturnType))
                    {
                        if (!method.Name.EndsWith("Async", System.StringComparison.Ordinal))
                        {
                            Assert.Fail($"The method '{method}' must end with 'Async'");
                        }

                        // Ensure method that returns a Task takes a CancellationToken
                        var parameterInfo = method.GetParameters().LastOrDefault();
                        if (parameterInfo is null)
                        {
                            Assert.Fail($"The method '{method}' must have a parameter of type 'CancellationToken'");
                        }

                        if (parameterInfo.ParameterType != typeof(CancellationToken))
                        {
                           Assert.Fail($"The last parameter of method '{method}' must be of type 'CancellationToken' and named 'cancellationToken'");
                        }

                        if (!string.Equals(parameterInfo.Name, "cancellationToken", System.StringComparison.Ordinal))
                        {
                            Assert.Fail($"The parameter '{parameterInfo.Name}' of '{method}' must be named 'cancellationToken'");
                        }

                        // Ensure the parameter is optional
                        var attr = parameterInfo.GetCustomAttribute<OptionalAttribute>();
                        if (attr is null)
                        {
                            Assert.Fail($"The parameter '{parameterInfo.Name}' of '{method}' must be optional");
                        }
                    }

                    if (method.ReturnType.IsGenericType && typeof(GitLabCollectionResponse<>).IsAssignableFrom(method.ReturnType.GetGenericTypeDefinition()))
                    {
                        if (!method.Name.EndsWith("Async", System.StringComparison.Ordinal))
                        {
                            Assert.Fail($"The method '{method}' must end with 'Async'");
                        }
                    }
                }
            }
        }
    }
}
