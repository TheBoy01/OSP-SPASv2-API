namespace OSP.SPASv2.Domain.View
{
    public class qryBatchPaymentHdr
    {
        public string PONo { get; set; }        
        public string PayeeName { get; set; }
        public decimal Amount { get; set; }
        public string SalesInvoiceNo { get; set; }
        public DateTime SalesInvoiceDate { get; set; }
        public string DeliveryNo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ReferenceReceiptNo { get; set; }
        public decimal HPDeduction { get; set; }
        public decimal FreightAmount { get; set; }
    }
}
