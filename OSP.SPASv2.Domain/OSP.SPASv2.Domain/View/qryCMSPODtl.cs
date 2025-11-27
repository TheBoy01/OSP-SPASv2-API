namespace OSP.SPASv2.Domain.View
{
    public class qryCMSPODtl
    {
        public string FactoryCode { get; set; }
        public string PONo { get; set; } 
        public string CasketCode { get; set; }
        public int OrderQty { get; set; }
        public decimal POAmount { get; set; }
        public string AuditUser { get; set; }   
        public DateTime AuditDate { get; set; }
        public string EditUser { get; set; }    
        public DateTime EditDate { get; set; }
    }
}
