using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Consumer.Client
{
    public class ApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(string BaseUri = null)
        {
            _client = new HttpClient { BaseAddress = new Uri(BaseUri ?? "http://my-api") };
        }

        public async Task<BasicObject> GetBasicObject(string id)
        {
            var ReasonPhrase = string.Empty;

            var Request = new HttpRequestMessage(HttpMethod.Get, "/basic/" + id);
            Request.Headers.Add("Accept", "application/json");

            var Response = await _client.SendAsync(Request);

            var Content = await Response.Content.ReadAsStringAsync();
            var Status = Response.StatusCode;

            ReasonPhrase = Response.ReasonPhrase;

            Request.Dispose();
            Response.Dispose();

            if (Status == HttpStatusCode.OK)
            {
                return !String.IsNullOrEmpty(Content)
                    ? JsonConvert.DeserializeObject<BasicObject>(Content)
                    : null;
            }
            throw new Exception(ReasonPhrase);
        }
    }
}
