using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace OSP.SPASv2.Service.Utility
{
    public class UtilitiesSched
    {
        private readonly IConfiguration configuration;
        private UtilitiesSched(IConfiguration _configuration)
        {
            configuration = _configuration;
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
                sb.Append(parameter.Value);
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
