using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using ClosedXML.Excel;

namespace OSP.Common.Service.Utility
{
    public class Utilities
    {

        private readonly IConfiguration configuration;
        private Utilities (IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public static DataTable ConvertToSystemDataTable<T>(List<T> dataList)
        {
            DataTable dataTable = new DataTable();

            if (dataList == null || dataList.Count == 0)
            {
                return dataTable;
            }

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, property.PropertyType);
            }

            foreach (T data in dataList)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    dataRow[property.Name] = property.GetValue(data);
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public static DataTable ConvertToOpenXmlDataTable<T>(List<T> dataList)
        {
            DataTable openXmlDataTable = new DataTable();

            // Your conversion logic for DocumentFormat.OpenXml.DataTable goes here

            return openXmlDataTable;
        }

        public static DataTable ConvertListToDataTable<T>(List<T> dataList)
        {
            DataTable dataTable = new DataTable();

            if (dataList == null || dataList.Count == 0)
            {
                return dataTable;
            }

            // Get the properties of the class using reflection
            PropertyInfo[] properties = typeof(T).GetProperties();

            // Create columns in DataTable based on the properties of the class
            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, property.PropertyType);
            }

            // Create rows in DataTable for each item in the list
            foreach (T data in dataList)
            {
                DataRow dataRow = dataTable.NewRow();

                // Populate the DataRow with values from the class properties
                foreach (PropertyInfo property in properties)
                {
                    dataRow[property.Name] = property.GetValue(data);
                }

                // Add the DataRow to the DataTable
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public static DataTable ConvertToDataTable<T>(T data)
        {
            DataTable dataTable = new DataTable();

            // Get the properties of the class using reflection
            PropertyInfo[] properties = typeof(T).GetProperties();

            // Create columns in DataTable based on the properties of the class
            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, property.PropertyType);
            }

            // Create a new DataRow and populate it with the values from the class properties
            DataRow dataRow = dataTable.NewRow();
            foreach (PropertyInfo property in properties)
            {
                dataRow[property.Name] = property.GetValue(data);
            }

            // Add the DataRow to the DataTable
            dataTable.Rows.Add(dataRow);

            return dataTable;
        }

        public static string GetmethodName([CallerMemberName] string methodname = null)
        {
            return methodname;// Console.WriteLine(methodname);
        }

        //public string GetIP()
        //{
        //     var config = configuration["profiles:SPASv2:applicationUrl"];
        //    return config;
        //}

        public static string Getprojectname()
        {
            string projectname = Assembly.GetExecutingAssembly().GetName().Name;
            return projectname;
        }

        public static string EncryptPW(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= hash.Length - 1; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static async Task<TblResponse> UploadFiles(List<IFormFile> files, string ReferenceNo, string UploadingFilePath)
        {

            TblResponse _TblResponse = new TblResponse();

            try
            {
                long size = files.Sum(f => f.Length);

                var filePaths = new List<string>();


                _TblResponse.MethodName = "Uploading Attachment";
                _TblResponse.TrxNo = ReferenceNo;

                foreach (var formFile in files)
                {
                    var fileName = System.IO.Path.GetFileName(formFile.FileName);

                    // Get file path to be uploaded
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), prno);
                    var path = Path.Combine(UploadingFilePath, ReferenceNo);
                    var filePath = Path.Combine(UploadingFilePath, ReferenceNo, fileName);
                    //var filePath = "C:\\Wa\\sample\\" + fileName;

                    // Check If file with same name exists and delete it
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Create a new local file and copy contents of uploaded file
                    using (var localFile = System.IO.File.OpenWrite(filePath))
                    using (var uploadedFile = formFile.OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                    }
                }

            }
            catch (Exception ex)
            {
                _TblResponse.ErrorMessage = ex.Message;
            }

            return _TblResponse;
        }

    }
}
