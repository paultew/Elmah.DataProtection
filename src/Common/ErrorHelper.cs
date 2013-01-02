namespace Elmah.Masking
{
    public static class ErrorHelper
    {
        public static void Obscure(Error error, MaskedValuesConfigurationSection configuration)
        {
            if (configuration.RemoveAspxAuth)
            {
                RequestCleaner.ObscureCookieValue(error.Cookies, error.ServerVariables, MaskedValuesConfigurationSection.AspxAuthCookie, configuration.ReplacementText);
            }

            foreach (MaskedItemElement obfuscatedItemElement in configuration.Cookies)
            {
                RequestCleaner.ObscureCookieValue(error.Cookies, error.ServerVariables, obfuscatedItemElement.Name, configuration.ReplacementText);
            }

            foreach (MaskedItemElement obfuscatedItemElement in configuration.FormVariables)
            {
                RequestCleaner.ObscureFormValue(error.Form, obfuscatedItemElement.Name, configuration.ReplacementText);
            }

            foreach (MaskedItemElement obfuscatedItemElement in configuration.ServerVariables)
            {
                RequestCleaner.ObscureServerVariable(error.ServerVariables, obfuscatedItemElement.Name, configuration.ReplacementText);
            }
        }
    }
}
