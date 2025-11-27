namespace OSP.SPASv2.Domain.References
{
    public class RefRegion
    {
        public string RegionCode { get; set; }

        public string RegionName { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }
    }
}
