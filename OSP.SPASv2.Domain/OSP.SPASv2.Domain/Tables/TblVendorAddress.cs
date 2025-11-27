using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendorAddress
    {
        [Key]
        public int idx { get; set; }


        public string VendorCode { get; set; }

        public string AddressType { get; set; }

        public string FullAddress { get; set; }

        public string AddressNo { get; set; }

        public string Street { get; set; }

        public string Brgy { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string ZipCode { get; set; }

        public bool Active { get; set; }

        public bool IsDefault { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }



    }
}
