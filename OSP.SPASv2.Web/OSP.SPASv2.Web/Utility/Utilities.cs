using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using OSP.SPASv2.Web.Models;
using Microsoft.Extensions.Primitives;
using static OSP.SPASv2.Web.Utility.Utilities;
using NuGet.Protocol;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using System;
using OSP.Common.Domain.Tables;
using System.Data.OleDb;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
//using Microsoft.AspNetCore.Mvc.Rendering;

namespace OSP.SPASv2.Web.Utility
{

    

    public static class ModelBuilder1
    {
        // A static method to build and configure a generic model with optional Parameter2
        public static GenericModel<T1, T2> BuildModel<T1, T2>(T1 parameter1)
        {
            // Create a new instance of the generic model with Parameter2 set to default
            GenericModel<T1, T2> model = new GenericModel<T1, T2>(parameter1, default(T2));

            // You can add more configuration or initialization logic here

            // Return the configured generic model
            return model;
        }

        public static GenericModel<T1, T2> BuildModel<T1, T2>(T1 parameter1, T2 parameter2)
        {
            // Create a new instance of the generic model and configure it
            GenericModel<T1, T2> model = new GenericModel<T1, T2>(parameter1, parameter2);

            // You can add more configuration or initialization logic here

            // Return the configured generic model
            return model;
        }

        
    }



    public static class JsonFetcher<T>
    {
        private static readonly HttpClient _httpClient;

        static JsonFetcher()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<T> GetJsonAsync(string apiUrl)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    throw new Exception($"Failed to fetch data. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }
    }


    public class Utilities
    {
        
        private readonly IConfiguration configuration;
        private Utilities(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        // Define a generic static utility class
        public static class GenericUtility<TModel>
        {
            // Static method to perform some action using the model
            public static void PerformAction(TModel model)
            {
                Console.WriteLine($"Performing action using {typeof(TModel)}");
                // You can perform specific actions using the model here
            }
        }

        public static string GetUrlWithQueryString(string requestUrl,
        Dictionary<string, string> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUrl);
            foreach (var parameter in queryStringParams)
            {
                if (parameter.Value == null)
                {
                    continue;
                }

                sb.Append(startingQuestionMarkAdded ? '&' : '?');
                sb.Append(parameter.Key);
                sb.Append('=');
                sb.Append(parameter.Value.Replace("&", "%26"));
                startingQuestionMarkAdded = true;
            }
            return sb.ToString();
        }

        public static string GetUrlWithQueryStringDateTime(string requestUrl,
       Dictionary<string, DateTime> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUrl);
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

        public static string GetmethodName([CallerMemberName] string methodname = null)
        {
            return methodname;// Console.WriteLine(methodname);
        }

        public string GetIP()
        {
            var config = configuration["profiles:SPASv2:applicationUrl"];
            return config;
        }

        public static string Getprojectname()
        {
            string projectname = Assembly.GetExecutingAssembly().GetName().Name;
            return projectname;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString, string SheetName, ref string process)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
               
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + SheetName + "$]", oledbConn))
                {
                    process = "opening";
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
                process = process + " complete";
            }
            catch (Exception err)
            {
                process = "failed " + err.Message;
                if (err.Message.Contains(SheetName) && err.Message.Contains("is not a valid name"))
                {
                    throw new Exception(SheetName + " sheet does not exist in your file.");
                }

            }
            finally
            {

                oledbConn.Close();
            }

            return dt;

        }

        public static string GenerateBitMap(string path)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                Bitmap bitMapImg = (Bitmap)Image.FromFile(path, true);

                using (Bitmap bitMap = bitMapImg)
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    return "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

        }

        public static string EncodeBase64(string source)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(source);
            string encodedText = Convert.ToBase64String(plainTextBytes);
            return encodedText;
        }

        public static string DecodeBase64(string source)
        {
            source = source.Replace(" ", "+");
            var encodedTextBytes = Convert.FromBase64String(source);
            string plainText = Encoding.UTF8.GetString(encodedTextBytes);
            return plainText;
        }


        //public static async Task<string> RenderPartialViewToStringAsync(Controller controller, string viewName, object model)
        //{
        //	if (string.IsNullOrEmpty(viewName))
        //		viewName = controller.ControllerContext.ActionDescriptor.ActionName;

        //	controller.ViewData.Model = model;

        //	using (var sw = new StringWriter())
        //	{
        //		var viewResult = await controller.ViewEngine.FindViewAsync(controller.ControllerContext, viewName, false);
        //		var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw, new HtmlHelperOptions());
        //		await viewResult.View.RenderAsync(viewContext);

        //		return sw.ToString();
        //	}
        //}



    }

}
