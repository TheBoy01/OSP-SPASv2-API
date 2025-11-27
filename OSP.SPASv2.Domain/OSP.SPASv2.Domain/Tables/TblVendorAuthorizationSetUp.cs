namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendorAuthorizationSetUp
    {

        public string VendorCode { get; set; }

        public string Authorizer { get; set; }

        public int? AuthorizeLevel { get; set; }

        public string AuditUser { get; set; }

        public DateTime? AuditDate { get; set; }

    }
}
