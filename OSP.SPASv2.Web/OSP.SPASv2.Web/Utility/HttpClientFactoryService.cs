using System.Text.Json;
using Microsoft.Extensions.Http;

namespace OSP.SPASv2.Web.Utility
{
    public class HttpClientFactoryService : IHttpClientServiceImplementation
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<RefCompany>> GetCompanieswithHttpClientFactory()
        {
            var httpClient = _httpClientFactory.CreateClient("SPASv2RepoClient");
            using (var response = await httpClient.GetAsync("companies", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                //var stream = await response.Content.ReadAsStreamAsync();

                //var companies = await JsonSerializer.Deserialize<List<RefCompany>>(stream, _options);
                //return companies;

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var companies = await JsonSerializer.DeserializeAsync<List<RefCompany>>(stream, _options);
                    return companies;
                }
            }
        }
    }



}
