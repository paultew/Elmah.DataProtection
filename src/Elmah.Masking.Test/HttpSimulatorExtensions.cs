namespace Elmah.Masking.Test
{
    using Subtext.TestLibrary;

    public static class HttpSimulatorExtensions
    {
        public static HttpSimulator SetCookies(this HttpSimulator httpSimulator, string cookies)
        {
            httpSimulator.SetHeader("ALL_HTTP", string.Format("HTTP_COOKIE:{0}\r\n", cookies));
            httpSimulator.SetHeader("ALL_RAW", string.Format("Cookie: {0}\r\n", cookies));
            httpSimulator.SetHeader("HTTP_COOKIE", cookies);

            return httpSimulator;
        }
    }
}
