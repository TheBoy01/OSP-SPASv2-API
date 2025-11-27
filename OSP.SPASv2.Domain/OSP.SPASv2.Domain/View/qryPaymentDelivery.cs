namespace OSP.SPASv2.Domain.View
{
    public class qryPaymentDelivery
    {

        public string PONo { get; set; }
        //public string PayeeName { get; set; }
        public string ChapelCode { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DRNo{ get; set; }
        public string ReferenceReceipt { get; set; }
        public int Qty { get; set; }
        public string ItemDesc { get; set; }
        public string UserID { get; set; }
    }
}
