namespace OSP.SPASv2.Domain.View
{
    public class qryRequisition
    {

        //public string ReqNo { get; set; }
        public string UserCompanyCode { get; set; }
        public string UserDeptCode { get; set; }
        public DateTime RequestDate { get; set; }
        public string PayClassCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorDesc { get; set; }
        public string PayeeName { get; set; }
        public string PayMethodCode { get; set; }
        public string BankCode { get; set; }
        public string Destination { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string RefNo { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyDesc { get; set; }
        public string CompanyType { get; set; }
        public string DeptCode { get; set; }
        public string DeptDesc { get; set; }
        public string ItemDesc { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Gross { get; set; }
        public decimal VatRate { get; set; }
        public decimal VAT { get; set; }
        public decimal NetOfVAT { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Discount { get; set; }
        public decimal DtlTotalAmount { get; set; }
        public string AuditUser { get; set; }
        public decimal Deduction { get; set; }
        public bool isVendorVat { get; set; }


    }
}
