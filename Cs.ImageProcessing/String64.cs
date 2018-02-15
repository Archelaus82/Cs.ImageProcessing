using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Cs.ImageProcessing
{
    public class String64
    {
        private string _str;

        public String64(string str) 
        { 
            this._str = str.IsBase64Encoded() ? str : str.Base64Encode(); 
        }

        public static explicit operator string(String64 str) { return str._str; }
        public static explicit operator String64(string str) { return new String64(str); }

        public string Decode { get { return ((string)this._str).Base64Decode(); } }
    }

    public static class Extensions
    {
        public static string Base64Encode(this string utf16str)
        {
            byte[] data = Encoding.Unicode.GetBytes(utf16str);
            return Convert.ToBase64String(data);
        }

        public static string Base64Decode(this string utf64str)
        {
            byte[] data = Convert.FromBase64String(utf64str);
            return Encoding.Unicode.GetString(data);
        }

        public static bool IsBase64Encoded(this string str)
        {
            const string REGEX_PATTERN = @"^[a-zA-Z0-9\+/]*={0,3}$";

            str = str.Trim();
            return (str.Length % 4 == 0) & Regex.IsMatch(str, REGEX_PATTERN);
        }
    }
}
