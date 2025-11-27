namespace OSP.SPASv2.Domain.References
{
    public class RefContactType
    {

        public string ContactCode { get; set; }

        public string ContactType { get; set; }

        public bool Active { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
