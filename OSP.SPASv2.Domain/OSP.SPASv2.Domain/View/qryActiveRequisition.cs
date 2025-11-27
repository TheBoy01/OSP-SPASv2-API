namespace OSP.SPASv2.Domain.View
{
    public class qryActiveRequisition
    { 
        public string Reqno { get; set; }

        public string MainReqNo { get; set; }

        public string BatchNo { get; set; }

        public string PayClassCode { get; set; }

        public string VendorCode { get; set; }

        public string PayMethodCode { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal? AmountDue { get; set; }

        public decimal? TotalFreight { get; set; }

        public string SalesInvoiceNo { get; set; }

        public string DeliveryNo { get; set; }


    }
}
