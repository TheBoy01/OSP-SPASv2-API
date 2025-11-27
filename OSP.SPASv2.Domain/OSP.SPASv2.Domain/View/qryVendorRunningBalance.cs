namespace OSP.SPASv2.Domain.View
{
    public class qryVendorRunningBalance
    {
        public string VendorCode { get; set; }
        public string VendorName { get; set; }        
        public int OrigQty { get; set; }
        public int ApprovedQty { get; set; }        
        public int BalanceQty { get; set; }
        public int PendingQty { get; set; }
        public int TempBalanceQty { get; set; }
    }
}
