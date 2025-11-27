namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendorContactPerson
    {

        public string ContactPersonID { get; set; }

        public string VendorCode { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        public string DisplayName { get; set; }

        public bool IsDefault { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
