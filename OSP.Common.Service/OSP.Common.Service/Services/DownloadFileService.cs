using Core6_FileDownload.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace OSP.Common.Service.Services
{
    public class DownloadFileService
    {

        public async Task<FileDownload> DownloadFile1(string path)
        {
            try
            {
                FileDownload fileDownload = new FileDownload();
                //byte[] bytes;

                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                {
                    fileDownload.bytes = new byte[fileStream.Length];
                    await fileStream.ReadAsync(fileDownload.bytes, 0, (int)fileStream.Length);
                }

                //fileDownload.contentType = "";
                //new FileExtensionContentTypeProvider().TryGetContentType(path, out fileDownload.contentType);
                fileDownload.contentType = "";
                string contentType; // Declare a variable to store the output
                if (new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType))
                {
                    fileDownload.contentType = contentType;
                }

                // Ensure the file content type is set to a known type (e.g., "application/octet-stream")
                fileDownload.contentType = string.IsNullOrEmpty(fileDownload.contentType) ? "application/octet-stream" : fileDownload.contentType;
                return fileDownload;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }

       
    }
}
