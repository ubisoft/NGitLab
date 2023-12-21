using System.Globalization;
using System.Text;

namespace NGitLab.Mock;

internal static class Slug
{
    public static string Create(string text)
    {
        text = text.Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder(text.Length);
        for (var index = 0; index < text.Length; index++)
        {
            var ch = text[index];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (IsAllowed(ch))
            {
                sb.Append(char.ToLowerInvariant(ch));
            }
            else if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                sb.Append('-');
            }
        }

        text = sb.ToString();
        return text.Normalize(NormalizationForm.FormC);
    }

    private static bool IsAllowed(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '.' || c == '_';
    }
}
