using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using System;
using System.Net.Http.Headers;

namespace WebTeploobmenAppClient.Helpers
{
    public class RequestHelper
    {
        public static HttpClient PrepareClient(Uri uri, string? token)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public static string GetToken(HttpContext context) => context.User.FindFirst("Token").Value;
    }
}
