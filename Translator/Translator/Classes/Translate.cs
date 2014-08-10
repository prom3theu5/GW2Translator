using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Utilities
{
    public static class Translator
    {
        public static string TranslateGoogle(string text, string fromCulture, string toCulture)
        {
            fromCulture = fromCulture.ToLower();
            toCulture = toCulture.ToLower();

            string[] tokens = fromCulture.Split('-');
            if (tokens.Length > 1)
                fromCulture = tokens[0];

            tokens = toCulture.Split('-');
            if (tokens.Length > 1)
                toCulture = tokens[0];

            string url = string.Format(@"http://translate.google.com/translate_a/t?client=j&text={0}&hl=en&sl={1}&tl={2}",
                                       HttpUtility.UrlEncode(text), fromCulture, toCulture);

            string html = null;
            try
            {
                WebClient web = new WebClient();

                web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
                web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");

                web.Encoding = Encoding.UTF8;
                html = web.DownloadString(url);
            }
            catch (Exception ex)
            {
                return null;
            }

            string result = Regex.Match(html, "trans\":(\".*?\"),\"", RegexOptions.IgnoreCase).Groups[1].Value;

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Deserialize(result, typeof(string)) as string;
        }
    }
}