using OSP.Common.Domain.References;
using OSP.Common.Domain.View;
using System;
using System.Net.Http;
using System.Text.Json;

namespace OSP.Common.Domain.APIFactory.OSPService
{
    public class HttpOSPService:IHttpOSPService
    {
        private readonly IHttpClientFactory _repo;
        private readonly JsonSerializerOptions _options;
        private static readonly HttpClient _httpClient = new HttpClient();
       

        public HttpOSPService(IHttpClientFactory repo)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7090/api/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
            _repo = repo;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
        }

        public async Task<List<qryCompanyType>> GetCompanies(string url)
        {
            var client = _repo.CreateClient("OSPServiceClient");
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
            //    using (var response = await _httpClient.GetAsync("OSPCommon/GetCompanies", HttpCompletionOption.ResponseHeadersRead))
            //{
                //using (var client = new HttpClient())
                //{
                //var response = await client.GetAsync("https://localhost:7090/api/OSPCommon/GetCompanies");
                //response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var companies = await JsonSerializer.DeserializeAsync<List<qryCompanyType>>(stream, _options);
                        return companies;
                    }

                    //JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    //T result = await response.Content.ReadAsAsync<T>();
                    // var responseJson = await response.Content.ReadAsStreamAsync();
                    // var result =  JsonSerializer.Deserialize<List<RefCompany>(responseJson, _options);

                    //return result;
                    //T result = await response.Content.ReadAsAsync();
                    // return result;

                }
                else
                {
                    // Handle error cases here, throw exception or return default value based on your use case
                    // For example, you can throw an exception with the error message
                    throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }


                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var companies = await JsonSerializer.DeserializeAsync<List<qryCompanyType>>(stream, _options);
                    return companies;
                }
                //var stream = await response.Content.ReadAsStreamAsync();
                //var companies = await JsonSerializer.Deserialize<List<RefCompany>>(stream, _options);
                //return companies;
            }

        }


    }
}
