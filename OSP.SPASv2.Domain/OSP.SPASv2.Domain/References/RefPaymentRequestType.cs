namespace OSP.SPASv2.Domain.References
{
    public class RefPaymentRequestType
    { 
        public int RequestType { get; set; }

        public string RequestDesc { get; set; }

        public bool? IsUserInput { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public bool UploadStat { get; set; }

        public string EditUser { get; set; }

        public DateTime EditDate { get; set; } 

    }
}
