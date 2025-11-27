namespace OSP.SPASv2.Domain.References
{
    public class RefATC
    {
        public string ATCCode { get; set; }

        public string ATCDesc { get; set; }

        public string ATCType { get; set; }

        public decimal TaxRate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public bool UploadStat { get; set; }

        public string EditUser { get; set; }

        public DateTime EditDate { get; set; }

    }
}
