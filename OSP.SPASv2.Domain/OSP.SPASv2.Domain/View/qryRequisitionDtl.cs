namespace OSP.SPASv2.Domain.View
{
    public class qryRequisitionDtl
    {
        public string ReqNo { get; set; }
        public decimal Gross { get; set; }
      //  public decimal VatRate { get; set; }
        public decimal VAT { get; set; }
        public decimal NetOfVAT { get; set; }
        public decimal TotalTax { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount  { get; set; }
        public decimal Deduction { get; set; }
    }
}
