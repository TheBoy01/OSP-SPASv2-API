namespace OSP.SPASv2.Domain.References
{
    public class RefDocType
    {

        public string DocCode { get; set; }

        public string DocDesc { get; set; }

        public bool Active { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }


    }
}
