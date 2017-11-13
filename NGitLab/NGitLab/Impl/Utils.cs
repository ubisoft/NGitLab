using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NGitLab.Impl
{
    internal static class Utils
    {
        public static string AddParameter<T>(string url, string parameterName, T value)
        {
            if (Equals(value, null))
            {
                return url;
            }

            string @operator = !url.Contains("?") ? "?" : "&";
            var formattedValue = HttpUtility.UrlEncode(value.ToString());
            var parameter = $"{@operator}{parameterName}={formattedValue}";
            return url + parameter;
        }
    }
}
