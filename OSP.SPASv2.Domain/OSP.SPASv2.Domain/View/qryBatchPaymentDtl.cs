namespace OSP.SPASv2.Domain.View
{
    public class qryBatchPaymentDtl
    {
        public string PONo { get; set; }
        public string SalesInvoice { get; set; }
        public string Department { get; set; }
        public string ItemDescription { get; set; }
        public int Balance { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal FreightAmount { get; set; }
        public string ReferenceReceipt { get; set; }
        public string DeliveryNo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime SalesInvoiceDate { get; set; }
        public decimal TemPriceAmount { get; set; }

    }
}
