using System;
using System.Net;
using System.Text;

namespace DetentionSystemCore
{
    public class KCISAPI
    {
        private CookieContainer _cookieContainer = new CookieContainer();

        public static Uri base_uri = new UriBuilder("https://portal.kcisec.com/").Uri;

        private string _account;
        public string account
        {
            get => _account;
            set
            {
                _account = value;
                // Add DSAI cookie to _cookieContainer
                _cookieContainer = GetCookieContainerWithAccCookie(value);
            }
        }

        public KCISAPI(string account)
        {
            this.account = account;
        }

        public static bool GetRequest(Uri uri, CookieContainer cookieContainer, out string result)
        {
            try
            {
                result = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.CookieContainer = cookieContainer;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }

                    // Update cookies
                    cookieContainer.Add(response.Cookies);
                }

                return true;
            }
            catch (WebException)
            {
                result = null;
                return false;
            }
        }

        public static bool GetAPI(string account, out string result, Uri base_uri, string api = "/DSAI/")
            => GetRequest(new Uri(base_uri, api), GetCookieContainerWithAccCookie(account), out result);

        public static bool GetAPI(string account, out string result, string api = "/DSAI/")
            => GetAPI(account, out result, base_uri, api);

        public bool GetAPI(string api, out string result)
            => GetRequest(new Uri(base_uri, api), _cookieContainer, out result);

        public static bool PostRequest(Uri uri, CookieContainer cookieContainer, string data, out string result)
        {
            try
            {
                result = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookieContainer;

                byte[] byteData = Encoding.UTF8.GetBytes(data);
                request.ContentLength = byteData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteData, 0, byteData.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }

                    // Update cookies
                    cookieContainer.Add(response.Cookies);
                }

                return true;
            }
            catch (WebException)
            {
                result = null;
                return false;
            }
        }

        public static bool PostAPI(string account, string data, out string result, Uri base_uri, string api = "/DSAI/")
            => PostRequest(new Uri(base_uri, api), GetCookieContainerWithAccCookie(account), data, out result);

        public static bool PostAPI(string account, string data, out string result, string api = "/DSAI/")
            => PostAPI(account, data, out result, base_uri, api);

        public bool PostAPI(string api, string data, out string result)
            => PostRequest(new Uri(base_uri, api), _cookieContainer, data, out result);

        public static bool TestAPI(string account, Uri base_uri)
        {
            if (GetAPI(account, out string result, base_uri, "/DSAI/"))
            {
                return result.Contains("[登出]");
            }
            return false;
        }

        public static bool TestAPI(string account)
            => TestAPI(account, base_uri);

        public bool TestAPI()
            => TestAPI(account, base_uri);

        private static CookieContainer GetCookieContainerWithAccCookie(string account)
        {
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie("DSAI", account, "/DSAI/", base_uri.Host));
            return cookieContainer;
        }
    }
}
