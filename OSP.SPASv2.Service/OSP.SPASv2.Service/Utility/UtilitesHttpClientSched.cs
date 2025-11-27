using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
//using Newtonsoft.Json;
using OSP.Common.Domain.Tables;

namespace OSP.SPASv2.Service.Utility
{
    public class UtilitesHttpClientSched<TEntity> where TEntity : class
    {

        private static readonly HttpClient client = new HttpClient();

        static UtilitesHttpClientSched()
        {
            // Optionally configure HttpClient here if needed
            //client.Timeout = TimeSpan.FromSeconds(30);
        }



        public static async Task<TblResponse> PostAsync(TEntity entity, string url)
        {
            try
            {
                //using (var client = new HttpClient())
                //{

                // Set the base URI of the API endpoint
                //client.BaseAddress = new Uri("https://example.com/api/");

                // Serialize the tmpPaymentRequestInventory object to JSON
                var json = JsonSerializer.Serialize(entity);
                //var json = JsonConvert.SerializeObject(entity);

                // Create a StringContent object with the JSON data
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a POST request to the API endpoint with the JSON data in the request body
                var response = await client.PostAsync(url, content);
                TblResponse tblResponse = null;

                if (response.IsSuccessStatusCode)
                {
                    //string json = await response.Content.ReadAsStringAsync();
                    //return JsonConvert.DeserializeObject<T>(json);

                    JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    // Read the response content as JSON and deserialize it to a TblResponse object
                    var responseJson = await response.Content.ReadAsStringAsync();
                    tblResponse = JsonSerializer.Deserialize<TblResponse>(responseJson, _options);
                    //tblResponse = JsonConvert.DeserializeObject<TblResponse>(responseJson);
                }
                else
                {
                    throw new Exception($"Failed to fetch data. Status code: {response.StatusCode}");
                }



                // Return the deserialized TblResponse object
                return tblResponse;
                //}
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public static async Task<T> PostAsyncEntity<TEntity, T>(TEntity entity, string url)
        {
            try
            {
                //using (HttpClient client =  new HttpClient())
                //{
                string jsonContent = JsonSerializer.Serialize(entity);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    T result = JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Use this option if you want case-insensitive property binding
                    });


                    return result;
                }
                else
                {
                    // Handle error cases here, throw exception or return default value based on your use case
                    // For example, you can throw an exception with the error message
                    throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
                //}

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public static async Task<T> PostAsyncT<T>(TEntity entity, string url)
        {
            try
            {
                //using (HttpClient client = new HttpClient())
                //{
                string jsonContent = JsonSerializer.Serialize(entity);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {

                    JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    //T result = await response.Content.ReadAsAsync<T>();
                    var responseJson = await response.Content.ReadAsStringAsync();
                    T result = JsonSerializer.Deserialize<T>(responseJson, _options);

                    //return result;
                    //T result = await response.Content.ReadAsAsync();
                    return result;
                }
                else
                {
                    // Handle error cases here, throw exception or return default value based on your use case
                    // For example, you can throw an exception with the error message
                    throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
                //}
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }



        public static async Task<IList<TEntity>> GetJsonlist(string url)
        {
            try
            {
                //using (var client = new HttpClient())
                //{
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();


                    JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


                    if (string.IsNullOrEmpty(content))
                    {
                        return default(IList<TEntity>);
                    }
                    else
                    {
                        var entlist = System.Text.Json.JsonSerializer.Deserialize<IList<TEntity>>(content, _options);
                        return entlist;
                    }
                }
                else
                {
                    throw new Exception($"Failed to fetch data. Status code: {response.StatusCode}");
                }



                //}
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }


        public static async Task<TEntity> GetJsonlist1(string url)
        {
            try
            {
                //using (var client = new HttpClient())
                //{
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();


                    JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    if (string.IsNullOrEmpty(content))
                    {
                        return default(TEntity);
                    }
                    else
                    {
                        var entlist = System.Text.Json.JsonSerializer.Deserialize<TEntity>(content, _options);
                        return entlist;
                    }
                }
                else
                {
                    throw new Exception($"Failed to fetch data. Status code: {response.StatusCode}");

                }



                //}
            }
            catch (Exception)
            {

                throw;
            }


        }



        public static async Task<string> GetJsonstring(string url)
        {
            try
            {
                //using (var client = new HttpClient())
                //{
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();


                    JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    // var entlist = System.Text.Json.JsonSerializer.Deserialize<string>(content, _options);
                    return content;
                }
                else
                {
                    throw new Exception($"Failed to fetch data. Status code: {response.StatusCode}");

                }

                //}
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }


    }

    public static class UtilitiesHttpClientExtensions
    {
        public static async Task<string> GetWithQueryStringAsync(string url,
            Dictionary<string, string> queryStringParams)
        {
            //var url = QueryHelpers.AddQueryString(uri, queryStringParams);
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();

                    return content.ToString();
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private static string GetUriWithQueryString(string requestUri,
            Dictionary<string, string> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUri);
            foreach (var parameter in queryStringParams)
            {
                if (parameter.Value == null)
                {
                    continue;
                }

                sb.Append(startingQuestionMarkAdded ? '&' : '?');
                sb.Append(parameter.Key);
                sb.Append('=');
                sb.Append(parameter.Value);
                startingQuestionMarkAdded = true;
            }
            return sb.ToString();
        }

        public static async Task<string> GetString(string url)
        {
            string a = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(url); //API controller name
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    //if (result != null)
                    //    var output = result;
                    a = result;
                }

                return a;
            }
        }
    }
}
