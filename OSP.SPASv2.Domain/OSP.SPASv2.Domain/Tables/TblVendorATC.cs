using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendorATC
    {

        public int Idx { get; set; }

        public string VendorCode { get; set; }

        public string ATCType { get; set; }

        public string ATCCode { get; set; }

        public bool Vattable { get; set; }

        public bool IsDefault { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }


    }
}
