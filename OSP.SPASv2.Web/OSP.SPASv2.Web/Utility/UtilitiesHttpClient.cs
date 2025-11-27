using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Http;

using System.Text;
using System.Text.Json;

using System.Threading.Tasks;

//using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;


//using Newtonsoft.Json;

namespace OSP.SPASv2.Web.Utility
{
    public static class UtilitiesHttpClient<TEntity> where TEntity : class
    {
        // private static readonly HttpClient client = new (new SocketsHttpHandler { PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1) });
       
       // private static readonly IHttpClientFactory _clientFactory; //= new IHttpClientFactory();
        private static readonly JsonSerializerOptions _options;
        static  UtilitiesHttpClient()
        {
           // _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public static async Task<T> PostAsyncTCF<T>(TEntity entity, string url, IHttpClientFactory _clientFactory)
        {
            try
            {
                //using (HttpClient client = new HttpClient())
                //{
                using (HttpClient client = _clientFactory.CreateClient())
                {
                    string jsonContent = JsonSerializer.Serialize(entity);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {

                        //JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            

        }



        public static async Task<TblResponse> PostAsync(TEntity entity, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(600000);

                    //using (HttpClient client = _clientFactory.CreateClient())
                    //{
                    //using (var response = await httpClient.GetAsync(url , HttpCompletionOption.ResponseHeadersRead))
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

                   //     JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

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
                }
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
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(600000);
                    //using (HttpClient client = _clientFactory.CreateClient())
                    //{
                    string jsonContent = JsonSerializer.Serialize(entity);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        T result = JsonSerializer.Deserialize<T>(responseContent,_options);


                        return result;
                    }
                    else
                    {
                        // Handle error cases here, throw exception or return default value based on your use case
                        // For example, you can throw an exception with the error message
                        throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }

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
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(600000);
                    //    using (HttpClient client = _clientFactory.CreateClient())
                    //{
                    string jsonContent = JsonSerializer.Serialize(entity);
                    HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {

//JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
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
                        throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}" );
                    }
                }
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
                using (var client = new HttpClient())
                {
                //    using (HttpClient client = _clientFactory.CreateClient())
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
                            var entlist = JsonSerializer.Deserialize<IList<TEntity>>(content, _options);
                            return entlist;
                        }
                    }
                    else
                    {
                        throw new Exception($"Failed to fetch data. Status code: {response.StatusCode}");
                    }



                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
      
        public static async Task<T> GetFromJsonAsync<T>(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //using (HttpClient client = _clientFactory.CreateClient())
                    //{
                    var response = await client.GetFromJsonAsync<T>(url);
                    return response;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }


        public static async Task<FileResult> DownloadFileFromApiAsync1(string apiUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //using (HttpClient client = _clientFactory.CreateClient())
                    //{
                    // Make a GET request to the API
                    var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode(); // Ensure the response indicates success (status code 200-299)

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a byte array
                        byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();

                        // Determine the content type based on the response headers
                        string contentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";

                        // Extract the file name from the Content-Disposition header
                        string fileName = response.Content.Headers.ContentDisposition?.FileNameStar ?? "DownloadedFile";

                        // Return a FileResult to initiate the file download
                        return new FileContentResult(fileBytes, contentType)
                        {
                            FileDownloadName = fileName
                        };
                    }
                    else
                    {
                        // Handle the error (e.g., log it, throw an exception, etc.)
                        // You might want to include more robust error handling based on your requirements
                        // Here, we return null to indicate that the file download was not successful
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // Here, we rethrow the exception, but you may want to log and handle it differently based on your application's requirements
                throw;
            }
        }


        public static async Task<FileResult> DownloadFileFromApiAsync(string apiUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //using (HttpClient client = _clientFactory.CreateClient())
                    //{
                    // Make a GET request to the API
                    var response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode(); // Ensure the response indicates success (status code 200-299)

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a byte array
                        var apiResponse = await response.Content.ReadAsStringAsync(); // Assuming GetFileDownload is the response type
                        JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                        var entlist = System.Text.Json.JsonSerializer.Deserialize<GetFileDownload>(apiResponse, _options);
                        if (entlist.Result == "OK" && entlist.File != null)
                        {
                            // Return a FileResult to initiate the file download
                            return new FileContentResult(entlist.File, entlist.MimeType)
                            {
                                FileDownloadName = entlist.FileName
                            };
                        }
                        else
                        {
                            // Handle the error from the API response
                            // You might want to include more robust error handling based on your requirements
                            // Here, we return null to indicate that the file download was not successful
                            return null;
                        }
                    }
                    else
                    {
                        // Handle the error (e.g., log it, throw an exception, etc.)
                        // You might want to include more robust error handling based on your requirements
                        // Here, we return null to indicate that the file download was not successful
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // Here, we rethrow the exception, but you may want to log and handle it differently based on your application's requirements
                throw;
            }
        }


        public static async Task<TEntity> GetJsonlist1(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //using (HttpClient client = _clientFactory.CreateClient())
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



                }
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
                using (var client = new HttpClient())
                {
                    //using (HttpClient client = _clientFactory.CreateClient())
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

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        public static async Task<Stream> GetFileStream(string fileUrl)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                Stream fileStream = await httpClient.GetStreamAsync(fileUrl);
                return fileStream;
            }
            catch (Exception ex)
            {
                return Stream.Null;
            }
        }

        public static async Task<TblResponse> DownloadFileAsync(string fileUrl, string destinationPath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //using (HttpClient client = _clientFactory.CreateClient())
                    //{
                    var response = await client.GetAsync(fileUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();

                        // Save the content to the specified destination path
                        await File.WriteAllBytesAsync(destinationPath, content);




                        //JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        //var entlist = JsonSerializer.Deserialize<TEntity>(content, _options);
                        var entlist = new TblResponse() { Status = "SUCCESS", ErrorMessage = "File downloaded successfully!" };
                        return entlist;
                        //return true;
                    }
                    else
                    {
                        // Handle the case where the request was not successful
                        //Console.WriteLine($"Failed to download file. Status code: {response.StatusCode}");
                        //return false;
                        var content = await response.Content.ReadAsStringAsync();
                        JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var entlist = JsonSerializer.Deserialize<TblResponse>(content, _options);
                        return entlist;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the HTTP request
                //Console.WriteLine($"Failed to download file. Exception: {ex.Message}");
                //return false;

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
