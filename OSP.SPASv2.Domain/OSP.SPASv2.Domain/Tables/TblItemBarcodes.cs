namespace OSP.SPASv2.Domain.Tables
{
    public class TblItemBarcodes
    {
        public string PONo { get; set; }
        public string BarCode { get; set; }
        public string ItemCode { get; set; }
        public string VendorCode { get; set; }
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
        public bool Cancel { get; set; }

    }
}
