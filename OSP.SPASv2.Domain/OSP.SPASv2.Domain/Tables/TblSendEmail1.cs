namespace OSP.SPASv2.Domain.Tables
{
    public class TblSendEmail1
    {
        public string From { get; set; }
        public IList<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IList<string> Attachment { get; set; }
        
        public IList<string> CCemails { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }    
        public string Password { get; set; }    
    }
}
