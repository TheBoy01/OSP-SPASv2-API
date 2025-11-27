namespace OSP.SPASv2.Domain.Tables
{
    public class TblRequisitionDtlSummary
    { 
        public int ReqNoDept { get; set; }

        public string ReqNo { get; set; }

        public string CompanyCode { get; set; }

        public string DeptCode { get; set; }

        public int Quantity { get; set; }

        public decimal Gross { get; set; }

        public decimal Vat { get; set; }

        public decimal NetofVat { get; set; }

        public decimal TotalTax { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal? Deduction { get; set; }

        public decimal Freight { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
