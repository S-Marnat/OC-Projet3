using System;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace P3.Resources.Models
{
    public static class Account
    {
        private static ResourceManager resourceManager = new ResourceManager("P3.Resources.Models.Account", Assembly.GetExecutingAssembly());
        private static CultureInfo resourceCulture;

        public static string ErrorMissingName
        {
            get
            {
                return resourceManager.GetString("ErrorMissingName", resourceCulture);
            }
        }
        public static string ErrorMissingPassword
        {
            get
            {
                return resourceManager.GetString("ErrorMissingPassword", resourceCulture);
            }
        }
    }
}
