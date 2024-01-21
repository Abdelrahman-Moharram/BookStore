using System.Text.RegularExpressions;

namespace BookStore.Helpers
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string text)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(text);
            if (match.Success)
                return true;
            return false;
        }
    }
}
