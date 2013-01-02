namespace Elmah.Masking.Test
{
    using System;
    using System.Web;

    using Elmah.Masking;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Subtext.TestLibrary;

    /// <summary>
    /// Summary description for ErrorObscureTests
    /// </summary>
    [TestClass]
    public class ErrorObscureTests
    {
        private const string Cookies = @"__utma=000000000.0000000000.0000000000.0000000000.0000000000.00; __utmz=000000000.0000000000.0.0.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); ASP.NET_SessionId=ltno2sazvumdr53rsumzh1sc; .ASPXAUTH=0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF0123456789ABCDEF01234567;";

        [TestMethod]
        public void TestAspxAuth()
        {
            var configurationSection = new MaskedValuesConfigurationSection();
            configurationSection.RemoveAspxAuth = true;
            configurationSection.ReplacementText = "OBSCURED";

            using (HttpSimulator simulator = new HttpSimulator("/", @"c:\inetpub\"))
            {
                simulator.SetCookies(Cookies)
                         .SimulateRequest(new Uri("http://localhost/"));

                var error = new Error(new HttpRequestValidationException(), HttpContext.Current);

                ErrorHelper.Obscure(error, configurationSection);

                Assert.AreEqual(configurationSection.ReplacementText, error.Cookies[MaskedValuesConfigurationSection.AspxAuthCookie]);
            }
        }

        [TestMethod]
        public void TestCookie()
        {
            const string CookieName = "ASP.NET_SessionId";

            var configurationSection = new MaskedValuesConfigurationSection();
            configurationSection.RemoveAspxAuth = false;
            configurationSection.ReplacementText = "OBSCURED";
            configurationSection.Cookies.Add(new MaskedItemElement(CookieName));

            using (HttpSimulator simulator = new HttpSimulator("/", @"c:\inetpub\"))
            {
                simulator.SetCookies(Cookies)
                         .SimulateRequest(new Uri("http://localhost/"));

                var error = new Error(new HttpRequestValidationException(), HttpContext.Current);

                Assert.IsNotNull(HttpContext.Current.Request.Cookies[CookieName]);

                ErrorHelper.Obscure(error, configurationSection);

                Assert.AreEqual(configurationSection.ReplacementText, error.Cookies[CookieName]);

                Assert.AreNotEqual(configurationSection.ReplacementText, error.Cookies[MaskedValuesConfigurationSection.AspxAuthCookie]);
            }
        }

        [TestMethod]
        public void TestFormVariable()
        {
            const string FormVariable = "Username";

            var configurationSection = new MaskedValuesConfigurationSection();
            configurationSection.RemoveAspxAuth = false;
            configurationSection.ReplacementText = "OBSCURED";
            configurationSection.FormVariables.Add(new MaskedItemElement(FormVariable));

            using (HttpSimulator simulator = new HttpSimulator("/", @"c:\inetpub\"))
            {
                simulator.SetFormVariable(FormVariable, "Admin").SimulateRequest(new Uri("http://localhost/"));

                var error = new Error(new HttpRequestValidationException(), HttpContext.Current);

                Assert.IsNotNull(HttpContext.Current.Request.Form[FormVariable]);

                ErrorHelper.Obscure(error, configurationSection);

                Assert.AreEqual(configurationSection.ReplacementText, error.Form[FormVariable]);
            }
        }

        [TestMethod]
        public void FormValueDoesNotExistTest()
        {
            const string FormValue = "RandomFormValue";

            var configurationSection = new MaskedValuesConfigurationSection();
            configurationSection.RemoveAspxAuth = false;
            configurationSection.ReplacementText = "OBSCURED";
            configurationSection.FormVariables.Add(new MaskedItemElement(FormValue));

            using (HttpSimulator simulator = new HttpSimulator("/", @"c:\inetpub\"))
            {
                simulator.SimulateRequest(new Uri("http://localhost/"), HttpVerb.GET);

                var error = new Error(new HttpRequestValidationException(), HttpContext.Current);

                Assert.IsNull(error.Form[FormValue]);
            }
        }

        [TestMethod]
        public void TestServerVariable()
        {
            const string ServerVariable = "REMOTE_USER";

            var configurationSection = new MaskedValuesConfigurationSection();
            configurationSection.RemoveAspxAuth = false;
            configurationSection.ReplacementText = "OBSCURED";
            configurationSection.ServerVariables.Add(new MaskedItemElement(ServerVariable));

            using (HttpSimulator simulator = new HttpSimulator("/", @"c:\inetpub\"))
            {
                simulator.SimulateRequest(new Uri("http://localhost/"));

                var error = new Error(new HttpRequestValidationException(), HttpContext.Current);

                Assert.IsNotNull(HttpContext.Current.Request.ServerVariables[ServerVariable]);

                ErrorHelper.Obscure(error, configurationSection);

                Assert.AreEqual(configurationSection.ReplacementText, error.ServerVariables[ServerVariable]);
            }
        }
    }
}
