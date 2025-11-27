using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.View
{
  
    public class qryPaymentRequestHdr
    {
        [Key]
        public string PRNo { get; set; }
        public DateTime RequestDate { get; set; }
        public string CompanyType { get; set; }
        public string CompanyCode { get; set; }
        public string DeptDesc { get; set; }
        public string DeptCode { get; set; }
        public string PayDesc { get; set; }
        public string DisplayName { get; set; }
        public string PayeeName { get; set; }
        public string PayMethodType { get; set; }
        public string BankName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string ReferenceNo { get; set; }
        public string Destination { get; set;}
    }
}
