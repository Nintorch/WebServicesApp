using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using System;

namespace WebTeploobmenAppClient.Helpers
{
    public class RequestHelper
    {
        public static HttpClient PrepareClient(Uri uri, IRequestCookieCollection? cookies)
        {
            HttpClient client = new();

            if (cookies != null && cookies.Count > 0)
            {
                CookieContainer cookieContainer = new();
                foreach (var cookie in cookies)
                {
                    cookieContainer.Add(uri, new Cookie(cookie.Key, cookie.Value));
                }
                client.DefaultRequestHeaders.Add("cookie", cookieContainer.GetCookieHeader(uri));
            }
            return client;
        }
    }
}
