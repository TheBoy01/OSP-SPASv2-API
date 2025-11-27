using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.View
{
    public class qryVendorDetails
    {
        [Key]
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public string PayeeName { get; set; }
        public string PaymethodCode { get; set; }
        public string PaymentMethod { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
       
        public bool isVat { get; set; }
    }
}
