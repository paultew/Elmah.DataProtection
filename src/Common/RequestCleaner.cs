namespace Elmah.Masking
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Web;

    internal static class RequestCleaner
    {
        private const string AllHttpServerVariable = "ALL_HTTP";
        private const string AllRawServerVariable = "ALL_RAW";
        private const string CookiesServerVariable = "HTTP_COOKIE";

        private static readonly string[] CookieVariables = { AllHttpServerVariable, AllRawServerVariable, CookiesServerVariable };

        public static void ObscureCookieValue(HttpContextBase context, string name, string newValue)
        {
            HttpCookie cookie = context.Request.Cookies.Get(name);
            if (cookie != null)
            {
                cookie.Value = newValue;
                context.Request.Cookies.Set(cookie);

                ObscureCookieServerVariable(context, name, newValue);
            }
        }

        public static void ObscureCookieValue(NameValueCollection cookieCollection, NameValueCollection serverVariableCollection, string name, string newValue)
        {
            var cookie = cookieCollection[name];
            if (cookie != null)
            {
                cookieCollection[name] = newValue;
                ObscureCookieServerVariable(serverVariableCollection, name, newValue);
            }
        }

        public static void ObscureFormValue(HttpContextBase context, string name, string newValue)
        {
            context.Request.Form[name] = newValue;
        }

        public static void ObscureFormValue(NameValueCollection formValueCollection, string name, string newValue)
        {
            formValueCollection[name] = newValue;
        }

        public static void ObscureServerVariable(HttpContextBase context, string name, string newValue)
        {
            context.Request.ServerVariables[name] = newValue;
        }

        public static void ObscureServerVariable(NameValueCollection formValueCollection, string name, string newValue)
        {
            formValueCollection[name] = newValue;
        }

        private static void ObscureCookieServerVariable(HttpContextBase context, string name, string newValue)
        {
            ObscureCookieServerVariable(context.Request.ServerVariables, name, newValue);
        }

        public static void ObscureCookieServerVariable(NameValueCollection serverVariableCollection, string name, string newValue)
        {
            foreach (var cookieVariable in CookieVariables)
            {
                string cookieServerVariableValue = serverVariableCollection[cookieVariable];
                if (!string.IsNullOrEmpty(cookieServerVariableValue))
                {
                    bool found = false;

                    List<string> cookieNameValuesList = new List<string>(cookieServerVariableValue.Split(new string[] { "; " }, StringSplitOptions.None));
                    for (int i = 0; i < cookieNameValuesList.Count; i++)
                    {
                        if (cookieNameValuesList[i].StartsWith(name + "=", StringComparison.OrdinalIgnoreCase))
                        {
                            found = true;
                            cookieNameValuesList.RemoveAt(i);
                            cookieNameValuesList.Insert(i, string.Format("{0}={1}", name, newValue));
                            break;
                        }
                    }

                    if (found)
                    {
                        serverVariableCollection[cookieVariable] = String.Join("; ", cookieNameValuesList.ToArray());
                    }
                }
            }
        }
    }
}
