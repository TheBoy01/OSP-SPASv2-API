namespace OSP.SPASv2.Domain.Tables
{
    public class TblPaymentRequestAttachment
    {

        public string PRNo { get; set; }

        public string DocCode { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string Link { get; set; }

        public string AuditUser { get; set; }

        public DateTime? AuditDate { get; set; }

    }
}
