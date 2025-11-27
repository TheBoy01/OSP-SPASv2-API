namespace OSP.SPASv2.Domain.View
{
    public class qryComputeReqDtlCriteria
    { 
        public int Qty { get; set; }
        public decimal Gross { get; set; }
        public decimal VatRate { get; set; }
        public decimal Discount { get; set; }
        public string DiscountCode { get; set; } 
    }
}
