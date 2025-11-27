namespace OSP.SPASv2.Domain.References
{
    public class RefGarden
    {

        public string BranchCode { get; set; }

        public string BranchDesc { get; set; }

        public string MotherBranchCode { get; set; }

        public string CompanyCode { get; set; }

        public string RegionCode { get; set; }

        public string TerritoryCode { get; set; }

        public string BranchClassCode { get; set; }

        public string Address { get; set; }

        public string PersonCode { get; set; }

        public bool Active { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
