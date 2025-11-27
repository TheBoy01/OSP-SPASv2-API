namespace OSP.SPASv2.Domain.View
{
    public class qryCMSPOHdr
    { 
        public string PONo { get; set; }
        public string FactoryCode { get; set; }
        public string ChapelCode { get; set; }
        public string CompanyCode { get; set; }
        public DateTime PODate { get; set; }
        public DateTime POReceivedDate { get; set; }
        public int Terms { get; set; }
        public string Remarks { get; set; }
        public decimal POAmount { get; set; }       
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
        public string EditUser { get; set; }
        public DateTime EditDate { get; set; }
        public bool Void { get; set; }
        public string VoidUser { get; set; }
        public DateTime VoidDate { get; set; }

    }
}
