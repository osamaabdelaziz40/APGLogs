using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APGLogs.DomainHelper.Services
{
    public static class ExtensionMethods
    {
        public static string EncodeBase64(this string value)
        {
            try
            {
                return string.IsNullOrEmpty(value) ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            }
            catch
            {
                return null;
            }
        }

        public static string DecodeBase64(this string value)
        {
            try
            {
                return string.IsNullOrEmpty(value) ? null : Encoding.UTF8.GetString(System.Convert.FromBase64String(value));
            }
            catch
            {
                return null;
            }
        }
    }
}