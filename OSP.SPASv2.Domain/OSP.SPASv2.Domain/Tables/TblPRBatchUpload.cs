namespace OSP.SPASv2.Domain.Tables
{
    public class TblPRBatchUpload
    {

        public string FileName { get; set; }

        public string BatchNo { get; set; }

        public string PRNo { get; set; }

        public decimal TotalAmount { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }
}
