namespace OSP.SPASv2.Domain.View
{
    public class qryBatchRequistion
    {
       public string CompanyType { get; set; }
        public string Department { get; set; }
        public string VendorName { get; set; } 
        public string ReferenceNo { get; set; }
        public string ItemDesc { get; set; }
        public int Qty { get; set; }
        public decimal Disc { get; set; }
        public string Remarks { get; set; }
        public decimal AmountPerUnit { get; set; }
    }
}
