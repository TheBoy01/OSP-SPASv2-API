namespace OSP.Common.Domain.Tables
{
    public class SMSModels
    {
        public class SMSApi
        {
            public string username { get; set; }
            public string stpeter { get; set; }
            public string password { get; set; }
            public string msisdn { get; set; }
            public string content { get; set; }
            public string shortcode_mask { get; set; }
            public string rcvd_transid { get; set; }
            public bool is_intl { get; set; }
        }

        public class SMSApiPldt
        {
            public string username { get; set; }
            public string password { get; set; }
            public string destination { get; set; }
            public string text { get; set; }
            public string source { get; set; }
        }

        public partial class tblSMSLog
        {
            public int IDNo { get; set; }
            public string Source { get; set; }
            public string Target { get; set; }
            public string StatusCode { get; set; }
            public string Message { get; set; }
            public Nullable<System.DateTime> AuditDate { get; set; }
        }
    }
}
