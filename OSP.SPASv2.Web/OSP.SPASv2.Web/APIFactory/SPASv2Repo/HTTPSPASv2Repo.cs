using Humanizer.Localisation;
using System.Text.Json;


namespace OSP.SPASv2.Web.APIFactory.SPASv2Repo
{
    public class HTTPSPASv2Repo : IHTTPSPASv2Repo
    {
        private readonly IHttpClientFactory _repo;
        private readonly JsonSerializerOptions _options;

        public HTTPSPASv2Repo(IHttpClientFactory repo)
        {
            _repo = repo;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        }

        public async Task Execute()
        {
            await GetSample();
        }

        public async Task<List<RefCompany>> GetSample()
        {
            var client = _repo.CreateClient("SPASv2RepoClient");
            using (var response = await client.GetAsync("api/CommonRepository/companies", HttpCompletionOption.ResponseHeadersRead))
            {

                //response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var companies = await JsonSerializer.DeserializeAsync<List<RefCompany>>(stream, _options);
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
                    var companies = await JsonSerializer.DeserializeAsync<List<RefCompany>>(stream, _options);
                    return companies;
                }
                //var stream = await response.Content.ReadAsStreamAsync();
                //var companies = await JsonSerializer.Deserialize<List<RefCompany>>(stream, _options);
                //return companies;
            }

        }


    }
}
