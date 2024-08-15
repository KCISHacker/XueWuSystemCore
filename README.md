# XueWuSystemCore

Methods in XueWuSystem.KCISAPI

| Method Signature                                                                 | Description                                                                                              |
|----------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------|
| `static bool SendRequest(Uri uri, CookieContainer cookieContainer, out string result)` | Send a request to a website with cookies.                                                                |
| `static bool GetAPI(string account, out string result, Uri base_uri, string api = "/DSAI/")` | Get API from a relevant path of `base_uri` with account cookie.                                          |
| `static bool GetAPI(string account, out string result, string api = "/DSAI/")`   | Get API from a relevant path of `base_uri` with account cookie.                                          |
| `bool GetAPI(string api, out string result)`                                     | Get API from a relevant path of `base_uri` with account cookie.                                          |
| `static bool TestAPI(string account, Uri base_uri)`                              | Test if the `base_uri` is a valid API and account is a valid account.                                     |
| `static bool TestAPI(string account)`                                            | Test if the `base_uri` is a valid API and account is a valid account.                                     |
| `bool TestAPI()`                                                                 | Test if the `base_uri` is a valid API and account is a valid account.                                     |
| `static Uri base_uri = new UriBuilder("https://portal.kcisec.com/").Uri`         | The base URI for the API.                                                                                |
| `string account`                                                                 | The account associated with the API.                                                                     |
| **Constructor** `KCISAPI(string account)`                                        | Initializes a new instance of the class with the specified account.                                       |