using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.References
{
    public class RefAddressType
    {

        [Key]
        public string AddressTypeCode { get; set; }

        public bool Active { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string AddressDesc { get; set; }

    }
}
