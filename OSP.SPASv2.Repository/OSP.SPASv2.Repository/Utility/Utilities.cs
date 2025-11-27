using ClosedXML;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Repository.Controllers;
using SPASv2.Context;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace OSP.SPASv2.Repository.Utility
{
    public class Utilities
    {
        private static StringBuilder sb = new StringBuilder();



        public static string GetEntityException(SPASv2Context ctx)
        {
            sb.Clear();

            foreach (var validationErrors in ctx.GetValidationErrors())
            {
                //foreach (var validationError in validationErrors.ValidationErrors)
                //{
                //    sb.AppendLine(validationError.ErrorMessage);
                //}
            }

            return sb.ToString();
        }

        //public static string GetmethodName([CallerMemberName] string methodname = null)
        //{
        //    return methodname;// Console.WriteLine(methodname);
        //}

        public static string GetCallingMethodName()
        {
            var st = new System.Diagnostics.StackTrace();
            var sf = st.GetFrame(1); // Index 0 would be GetCallingMethodName, 1 would be the catch block, 2 would be the caller method
            return sf.GetMethod().Name;
        }

        public static string GetmethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            string methodname = frame.GetMethod().Name;
            return methodname;
            // return MethodBase.GetCurrentMethod().Name ;// Console.WriteLine(methodname);
        }

        public static string GetMethodName1()
        {
            MethodBase method = new StackTrace().GetFrame(1)?.GetMethod();
            return method?.Name ?? "Method Not Found";
        }

        public static string Getprojectname()
        {
            string projectname = Assembly.GetExecutingAssembly().GetName().Name;
            return projectname;
        }

        public static string GetProjectName()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                return assembly.GetName().Name;
            }
            else
            {
                return "Unknown";
            }
        }


        public static string GetHttpAttName<TController>(string methodname)
        {
            Type type = typeof(TController);
            MethodInfo method = type.GetMethod(methodname);

            if (method == null)
            {
                HttpGetAttribute httpGetAttribute = method.GetCustomAttributes(typeof(HttpGetAttribute), false)
                   .FirstOrDefault() as HttpGetAttribute;
                return httpGetAttribute?.Name ?? "Method Not Found";
            }
            else
            {
                return "Method Not Found";
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
                sb.Append(parameter.Value);
                startingQuestionMarkAdded = true;
            }
            return sb.ToString();
        }

        public void ChangeConnection(string CompanyCode)
        { 
            
        }

        public static string ChangeItemDjango(string Item, bool Switch)
        {
            switch (Switch)
            {
                case true:
                    return Item.Replace("DJANGO METAL", "DJANGO IMPORTED");

                case false:
                    return Item.Replace("DJANGO IMPORTED", "DJANGO METAL");
            }
            
        }

    }


}
