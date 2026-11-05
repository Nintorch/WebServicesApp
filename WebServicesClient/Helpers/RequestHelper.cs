using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebTeploobmenAppClient.Helpers
{
    public static class RequestHelper
    {
        public static HttpClient PrepareClient(Uri uri, string? token)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public static string GetToken(HttpContext context) => context.User.FindFirst("Token").Value;

        public static async Task<HttpResponseMessage> GetAsync(string uriString, HttpContext context)
        {
            Uri uri = new(uriString);
            HttpClient client = PrepareClient(uri, GetToken(context));
            return await client.GetAsync(uri);
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync(string uriString, HttpContext context, object model)
        {
            Uri uri = new(uriString);
            HttpClient client = PrepareClient(uri, GetToken(context));
            return await client.PostAsJsonAsync(uri, model);
        }

        public static bool IsUnauthorized(Controller controller, HttpResponseMessage response, out IActionResult action)
        {
            action = null;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                action = controller.RedirectToAction("Logout", "Account");
                return true;
            }
            return false;
        }
    }
}
