namespace OSP.Common.Domain.Tables
{
    public class TblSendEmail
    {
        public string ReferenceNo { get; set; }
        public string SystemCode { get; set; }
        
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public IList<string> Attachment { get; set; } = new List<string>();
        public IList<string> CCemails { get; set; }
        public IList<string> BCemails { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
