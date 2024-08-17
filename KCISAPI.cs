using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DetentionSystemCore
{
    /// <summary>
    /// Provides methods to interact with the KCIS API.
    /// </summary>
    public class KCISAPI
    {
        private CookieContainer _cookieContainer = new CookieContainer();

        /// <summary>
        /// The base URI for the API.
        /// </summary>
        public static Uri base_uri = new UriBuilder("https://portal.kcisec.com/").Uri;

        private string _account;

        /// <summary>
        /// The account associated with the API.
        /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="KCISAPI"/> class with the specified account.
        /// </summary>
        /// <param name="account">The account associated with the API.</param>
        public KCISAPI(string account)
        {
            this.account = account;
        }

        /// <summary>
        /// Sends a GET request to a website with cookies.
        /// </summary>
        /// <param name="uri">The URI of the website.</param>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="result">The result of the GET request.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
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

        /// <summary>
        /// Sends a GET request to the API from a relevant path of <paramref name="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="account">The account associated with the API.</param>
        /// <param name="result">The result of the GET request.</param>
        /// <param name="base_uri">The base URI for the API.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        public static bool GetAPI(string api, string account, out string result, Uri base_uri)
            => GetRequest(new Uri(base_uri, api), GetCookieContainerWithAccCookie(account), out result);

        /// <summary>
        /// Sends a GET request to the API from a relevant path of <see cref="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="account">The account associated with the API.</param>
        /// <param name="result">The result of the GET request.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        public static bool GetAPI(string api, string account, out string result)
            => GetAPI(api, account, out result, base_uri);

        /// <summary>
        /// Sends a GET request to the API from a relevant path of <see cref="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="result">The result of the GET request.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        public bool GetAPI(string api, out string result)
            => GetRequest(new Uri(base_uri, api), _cookieContainer, out result);

        /// <summary>
        /// Sends a POST request to a website with cookies.
        /// </summary>
        /// <param name="uri">The URI of the website.</param>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="formData">The form data to be sent in the POST request.</param>
        /// <param name="result">The result of the POST request.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        public static bool PostRequest(Uri uri, CookieContainer cookieContainer, Dictionary<string, string> formData, out string result)
        {
            try
            {
                result = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.CookieContainer = cookieContainer;

                if (formData == null)
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                        {
                            result = reader.ReadToEnd();
                        }

                        // Update cookies
                        cookieContainer.Add(response.Cookies);
                    }
                }
                else
                {
                    request.ContentType = "multipart/form-data";

                    StringBuilder postData = new StringBuilder();
                    foreach (var kvp in formData)
                    {
                        postData.Append($"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}&");
                    }
                    byte[] byteData = Encoding.UTF8.GetBytes(postData.ToString().TrimEnd('&'));
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
                }

                return true;
            }
            catch (WebException)
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Sends a POST request to the API from a relevant path of <paramref name="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="account">The account associated with the API.</param>
        /// <param name="formData">The form data to be sent in the POST request.</param>
        /// <param name="result">The result of the POST request.</param>
        /// <param name="base_uri">The base URI for the API.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        public static bool PostAPI(string api, string account, Dictionary<string, string> formData, out string result, Uri base_uri)
            => PostRequest(new Uri(base_uri, api), GetCookieContainerWithAccCookie(account), formData, out result);

        /// <summary>
        /// Sends a POST request to the API from a relevant path of <see cref="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="account">The account associated with the API.</param>
        /// <param name="formData">The form data to be sent in the POST request.</param>
        /// <param name="result">The result of the POST request.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        public static bool PostAPI(string api, string account, Dictionary<string, string> formData, out string result)
            => PostAPI(api, account, formData, out result, base_uri);

        /// <summary>
        /// Sends a POST request to the API from a relevant path of <see cref="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="formData">The form data to be sent in the POST request.</param>
        /// <param name="result">The result of the POST request.</param>
        /// <returns>True if the request was successful; otherwise, false.</returns>
        public bool PostAPI(string api, Dictionary<string, string> formData, out string result)
            => PostRequest(new Uri(base_uri, api), _cookieContainer, formData, out result);

        /// <summary>
        /// Tests if the <paramref name="base_uri"/> is a valid API and account is a valid account.
        /// </summary>
        /// <param name="account">The account associated with the API.</param>
        /// <param name="base_uri">The base URI for the API.</param>
        /// <returns>True if the API and account are valid; otherwise, false.</returns>
        public static bool TestAPI(string account, Uri base_uri)
        {
            if (GetAPI("/DSAI/", account, out string result, base_uri))
            {
                return result.Contains("[登出]");
            }
            return false;
        }

        /// <summary>
        /// Tests if the <see cref="base_uri"/> is a valid API and account is a valid account.
        /// </summary>
        /// <param name="account">The account associated with the API.</param>
        /// <returns>True if the API and account are valid; otherwise, false.</returns>
        public static bool TestAPI(string account)
            => TestAPI(account, base_uri);

        /// <summary>
        /// Tests if the <see cref="base_uri"/> is a valid API and account is a valid account.
        /// </summary>
        /// <returns>True if the API and account are valid; otherwise, false.</returns>
        public bool TestAPI()
            => TestAPI(account, base_uri);

        /// <summary>
        /// Downloads a file from a website with cookies.
        /// </summary>
        /// <param name="cookieContainer">The cookie container.</param>
        /// <param name="uri">The URI of the file.</param>
        /// <param name="path">The path to save the downloaded file.</param>
        /// <returns>True if the file was downloaded successfully; otherwise, false.</returns>
        public static bool DownloadFile(CookieContainer cookieContainer, Uri uri, string path)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.CookieContainer = cookieContainer;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }

                    // Update cookies
                    cookieContainer.Add(response.Cookies);
                }

                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Downloads a file from a relevant path of <paramref name="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="account">The account associated with the API.</param>
        /// <param name="path">The path to save the downloaded file.</param>
        /// <param name="base_uri">The base URI for the API.</param>
        /// <returns>True if the file was downloaded successfully; otherwise, false.</returns>
        public static bool DownloadFile(string api, string account, string path, Uri base_uri)
            => DownloadFile(GetCookieContainerWithAccCookie(account), new Uri(base_uri, api), path);

        /// <summary>
        /// Downloads a file from a relevant path of <see cref="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="account">The account associated with the API.</param>
        /// <param name="path">The path to save the downloaded file.</param>
        /// <returns>True if the file was downloaded successfully; otherwise, false.</returns>
        public static bool DownloadFile(string api, string account, string path)
            => DownloadFile(api, account, path, base_uri);

        /// <summary>
        /// Downloads a file from a relevant path of <see cref="base_uri"/> with account cookie.
        /// </summary>
        /// <param name="api">The API path.</param>
        /// <param name="path">The path to save the downloaded file.</param>
        /// <returns>True if the file was downloaded successfully; otherwise, false.</returns>
        public bool DownloadFile(string api, string path)
            => DownloadFile(_cookieContainer, new Uri(base_uri, api), path);

        /// <summary>
        /// Gets a cookie container with the account cookie.
        /// </summary>
        /// <param name="account">The account associated with the API.</param>
        /// <returns>A cookie container with the account cookie.</returns>
        private static CookieContainer GetCookieContainerWithAccCookie(string account)
        {
            CookieContainer cookieContainer = new CookieContainer();
            cookieContainer.Add(new Cookie("DSAI", account, "/", base_uri.Host));
            return cookieContainer;
        }
    }
}
