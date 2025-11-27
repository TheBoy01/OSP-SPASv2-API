using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.View
{
    public class qryVendorList
    {

        [Key]
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public bool Active { get; set; }
        public string ContactDetails { get; set; }
        public string TIN { get; set; }
        public string ATC { get; set; }
        public string Address { get; set; }
        public string BankAcct { get; set; }
        //public string Item { get; set; }

    }
}
