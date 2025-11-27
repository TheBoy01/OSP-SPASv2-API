namespace OSP.SPASv2.Domain.References
{
    public class RefAccountMap
    {

        public int Idx { get; set; }

        public string PayClassCode { get; set; }

        public string NormalBalance { get; set; }

        public string AccountCode { get; set; }
        public string DeptAcctCode { get; set; }
        public string Formula { get; set; }
        public bool ForVat { get; set; }
        public int Hierarchy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }

    }
}
