using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSP.SPASv2.Domain.View
{
    public class qryPRAuthorizationList
    {
        [Key]
        public string Reqno { get; set; }

        public string ReqType { get; set; }
        public string CompanyCode { get; set; }

        public string CompanyDesc { get; set; }

        public string DeptCode { get; set; }
        public string VendorDesc { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ReqDate { get; set; }

        public string PayDesc { get; set; }

    }


}
