namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendorPayClass
    {

        public string VendorCode { get; set; }

        public string PayClassCode { get; set; }

        public int Terms { get; set; }

        public int POType { get; set; }

        public bool IsDefault { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
