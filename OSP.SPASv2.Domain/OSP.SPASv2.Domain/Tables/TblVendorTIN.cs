using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{ 
    public class TblVendorTIN
    {
        public string VendorCode { get; set; }

        public string TIN { get; set; }

        //public bool IsDefault { get; set; }

        //public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
