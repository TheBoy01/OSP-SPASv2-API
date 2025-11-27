using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendor
    {

        [Key]
        [Required]
        [DataMember]
        public string VendorCode { get; set; }

        public string VendorID { get; set; }

        public string VendorType { get; set; }

        public string DisplayName { get; set; }

        public string PayeeCVName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string FirstName { get; set; }   

        public string Suffix { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public bool isVAT { get; set; }
        public string Prefix { get; set; }

    }
}
