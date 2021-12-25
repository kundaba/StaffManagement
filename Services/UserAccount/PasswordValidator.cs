using System.Linq;

namespace CDFStaffManagement.Services.UserAccount
{
    public static class PasswordValidator
    {
        private static bool IsLowerCaseLetter(char c)
        {
            return c >= 'a' && c <= 'z';
        }
        private static bool IsUppercaseLetter(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        private static bool IsSymbol(char c)
        {
            return c > 32 && c < 127 && !IsDigit(c) && !IsLowerCaseLetter(c) && !IsUppercaseLetter(c) ;
        }
        public static bool IsValidPassword(string? password)
        {
            if(string.IsNullOrEmpty(password))
            {
                return false;
            }
            var result =
               password.Any(IsLowerCaseLetter) &&
               password.Any(IsDigit) &&
               password.Any(IsSymbol) &&
               password.Any(IsUppercaseLetter);

            return result;
        }
    }
}
