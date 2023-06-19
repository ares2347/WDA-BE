using System.Text.RegularExpressions;

namespace WDA.Shared;

public static class Helper
{
    public static bool ValidateEmailString(string email)
    {
        return Regex.IsMatch(email,
                @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,5})+)$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
    }
    
}