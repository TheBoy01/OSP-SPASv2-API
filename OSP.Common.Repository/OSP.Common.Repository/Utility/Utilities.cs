using OSP.Common.Repository.Context;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace OSP.Common.Repository.Utility
{
    public class Utilities
    {
        private static StringBuilder sb = new StringBuilder();

        public static string GetEntityException(OSPContext ctx)
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


        public static string GetmethodName([CallerMemberName] string methodname = null)
        {
            return methodname;// Console.WriteLine(methodname);
        }

        public static string Getprojectname()
        {
            string projectname = Assembly.GetExecutingAssembly().GetName().Name;
            return projectname;
        }
    }
}
