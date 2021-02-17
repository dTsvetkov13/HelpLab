using Microservices.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace APIGateway.Services
{
    public class HttpSender
    {
        private readonly IHttpClientFactory _clientFactory;
        public HttpSender(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ReturnType> SendPostAsync<ReturnType, MessageType>(MessageType message, string route)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, route);
            request.Content = JsonContent.Create(message);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            response = await client.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            ReturnType result = JsonConvert.DeserializeObject<ReturnType>(responseString);

            return result;
        }

        public async Task<ReturnType> SendGetAsync<ReturnType>(string route)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            response = await client.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            ReturnType result = JsonConvert.DeserializeObject<ReturnType>(responseString);

            return result;
        }

        public async Task<ReturnType> SendPutAsync<ReturnType, MessageType>(MessageType message, string route)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, route);
            request.Content = JsonContent.Create(message);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            response = await client.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            ReturnType result = JsonConvert.DeserializeObject<ReturnType>(responseString);

            return result;
        }

        public async Task<ReturnType> SendDeleteAsync<ReturnType>(string route)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, route);
            var client = _clientFactory.CreateClient();

            HttpResponseMessage response;

            response = await client.SendAsync(request);

            var responseString = await response.Content.ReadAsStringAsync();

            ReturnType result = JsonConvert.DeserializeObject<ReturnType>(responseString);

            return result;
        }
    }
}
