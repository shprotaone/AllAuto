
using System.Text;

namespace AllAuto.Domain.Extensions
{
    public static class StringExtension
    {
        public static string Join(this List<string> words)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < words.Count; i++)
            {
                sb.Append($"{i + 1}: {words[i]}");
            }

            return sb.ToString();
        }
    }
}
