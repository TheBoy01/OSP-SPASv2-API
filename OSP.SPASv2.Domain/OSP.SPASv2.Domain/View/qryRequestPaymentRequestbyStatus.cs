using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.View
{
    public class qryRequestPaymentRequestbyStatus
    {
        public string RequestID { get; set; }

        public string PONo { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string Vendor { get; set; }
        public DateTime RequestDate { get; set; }
        public string PayClass { get; set; }
        public string BatchPRNo { get; set; }
        public string CompanyType { get; set; }
        public string DeptDesc { get; set; }
        public string PayMethodType { get; set; }
        public string MainReqNo { get; set; }
        public decimal Deduction { get; set; }
        public string RefNo { get; set; }
        public string ItemCompany { get; set; }
        public int OrigQty { get; set; }
        public int ApprovedQty { get; set; }
        public int PendingQty { get; set; }
        public int TempBalanceQty { get; set; }
        public int BalanceQty { get; set; }
        public string TransType { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
