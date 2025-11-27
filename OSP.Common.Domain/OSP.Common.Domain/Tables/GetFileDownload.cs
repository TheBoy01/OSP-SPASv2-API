namespace OSP.Common.Domain.Tables
{
    

    public class GetFileDownload
    {
        public GetFileDownload()
        {
        }

        public string Result { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] File { get; set; }
    }
}
