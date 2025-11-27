namespace OSP.SPASv2.Domain.View
{
    public class qryRequisitionInfo
    {

        public string ReqNo { get; set; }
        public string MainReqNo { get; set; }
        public DateTime ReqDate { get; set; }
        public string RefNo { get; set; }
        public string Remarks { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        public string RequesterAddress { get; set; }
        public string RequesterCompanyType { get; set; }
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public string PayClass { get; set; }
        public string PayeeName { get; set; }
        public string VendorCode { get; set; }
        public string Vendor { get; set; }
        public string PayMethod { get; set; }
        public string PaymentChannel { get; set; }
        public string AccountNo { get; set; }
        public string Status { get; set; }
        public decimal Deduction { get; set; }
        public string TransType { get; set; }
        public string SalesInvoiceNo { get; set; }
        public DateTime? SalesInvoiceDate { get; set; }
        public string DeliveryNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public bool? Printed { get; set; }
        public decimal TotalFreight { get; set; }
        public string BatchNo { get; set; }
        public  string PayClassCode { get; set; }
        public string ItemCompany { get; set; }
        public string ReqApprovalNo { get; set; }
        public string MainReqApprovalNo { get; set; }
        public string RushReason { get; set; }
        public string RushRemarks { get; set; }
        public string DenialReason { get; set; }
        public string DenialRemarks { get; set; }
    }
}