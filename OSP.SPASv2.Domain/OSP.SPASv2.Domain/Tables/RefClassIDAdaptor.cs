namespace OSP.SPASv2.Domain.Tables
{
    public class RefClassIDAdaptor
    { 
        public string ClassID { get; set; }

        public string PayClassCode { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; } 
    }
}
