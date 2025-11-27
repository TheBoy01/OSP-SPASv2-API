using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSP.SPASv2.Domain.View
{
    public class qryPRBatchData
    { 
        public string FileName { get; set; }
        public string CompanyType { get; set;}
        public string DeptCode { get; set; } 
        public string PayeeName { get; set; }
        public string PayMethod { get; set; }
        public string BankName { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }

    }
}
