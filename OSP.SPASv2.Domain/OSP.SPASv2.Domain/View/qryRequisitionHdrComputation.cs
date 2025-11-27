namespace OSP.SPASv2.Domain.View
{
    public class qryRequisitionHdrComputation
    {
        public string ReqNo { get; set; }
        public decimal Gross { get; set; }

        //public decimal VatRate { get; set; }

        public decimal Vat { get; set; }

        public decimal NetOfVat { get; set; }

        public decimal TotalTax { get; set; }
        public decimal Discount { get; set; }
        public decimal Deduction { get; set; }

        public decimal AmountDue { get; set; }



    }
}
