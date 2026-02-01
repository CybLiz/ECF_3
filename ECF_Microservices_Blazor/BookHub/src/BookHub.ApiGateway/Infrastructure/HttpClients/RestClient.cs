using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace BookHubGateway.Infrastructure.HttpClients
{

    public class RestClient<TSend, TReceive>
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _options;

        public RestClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new HttpClient();

            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
            _options.Converters.Add(new JsonStringEnumConverter());
        }

        // GET list
        public async Task<List<TSend>> GetListRequest(string url)
        {
            var response = await _client.GetAsync(_baseUrl + url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TSend>>(json, _options)
                   ?? throw new Exception("Result null");
        }


        // GET single item
        public async Task<TSend> GetRequest(string url)
        {
            var response = await _client.GetAsync(_baseUrl + url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TSend>(json, _options)
                   ?? throw new Exception("Result null");
        }

        public void SetAuthorizationHeader(string? authorizationHeader)
        {
            _client.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrWhiteSpace(authorizationHeader))
            {
                _client.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(authorizationHeader);
            }
        }


        // POST avec body
        public async Task<TSend> PostRequest<TBody>(string url, TBody body)
        {
            StringContent? content = null;
            if (body != null)
            {
                var json = JsonSerializer.Serialize(body, _options);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            var response = await _client.PostAsync(_baseUrl + url, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TSend>(result, _options)
                   ?? throw new Exception("Result null");
        }

        // POST sans body
        public async Task<TSend> PostRequest(string url)
        {
            var response = await _client.PostAsync(_baseUrl + url, null);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TSend>(result, _options)
                   ?? throw new Exception("Result null");
        }

        // PUT avec generic body
        public async Task<TSend> PutRequest<TBody>(string url, TBody body)
        {
            var json = JsonSerializer.Serialize(body, _options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(_baseUrl + url, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TSend>(result, _options)
                   ?? throw new Exception("Result null");
        }

        // DELETE request
        public async Task DeleteRequest(string url)
        {
            var response = await _client.DeleteAsync(_baseUrl + url);
            response.EnsureSuccessStatusCode();
        }
    }
}
