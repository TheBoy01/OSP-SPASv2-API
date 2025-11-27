namespace OSP.SPASv2.Domain.View
{
    public class qryUpdateReqDetails
    {
        public string UserCode { get; set; }
        public string ReqNo { get; set; }
        public string SINo { get; set; }
        public DateTime ? SIDate { get; set; }
        public decimal Deduction { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Freight { get; set; }
    }
}
