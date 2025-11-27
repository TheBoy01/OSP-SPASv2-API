using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.References
{
    public class RefVendorType
    {
        [Key]
        public string VendorTypeCode { get; set; }

        public string VendorTypeDesc { get; set; }

        public bool Active { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
