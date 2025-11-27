namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendorDocRequired
    {

        public string VendorCode { get; set; }

        public string DocCode { get; set; }

        public string DocType { get; set; }

        public string FIleName { get; set; }

        public string Link { get; set; }

        public DateTime Validity { get; set; }

        public bool Active { get; set; }

        public bool IsDefault { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
