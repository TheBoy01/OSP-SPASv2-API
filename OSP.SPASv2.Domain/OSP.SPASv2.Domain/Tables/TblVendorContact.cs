using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    [DataContract]
    public class TblVendorContact
    {

        [Key]
        [Required]
        public int Idx { get; set; }

        public string ContactPersonID { get; set; }

        public string ContactCode { get; set; }

        public string ContactDetails { get; set; }

        public bool Active { get; set; }

        public bool IsDefault { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
