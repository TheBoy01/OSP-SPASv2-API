namespace OSP.SPASv2.Domain.References
{
    public class RefBrgy
    {

        public int Idx { get; set; }

        public string CityCode { get; set; }

        public string BrgyName { get; set; }
        public string ZipCode { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
