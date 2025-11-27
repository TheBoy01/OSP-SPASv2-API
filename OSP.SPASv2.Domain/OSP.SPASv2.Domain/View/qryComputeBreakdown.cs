namespace OSP.SPASv2.Domain.View
{
    public class qryComputeBreakdown
    {
        public int Qty { get; set; }
        public decimal Gross { get; set; }
        public decimal VatRate { get; set; }
        public decimal Vat { get; set; }
        public decimal NetOfVAT { get; set; }
        public decimal Discount { get; set; }
        public decimal AmountDue { get; set; }
        public string Disccode { get; set; }
        public bool isVAT { get; set; }


    }
}
