namespace OSP.SPASv2.Domain.Tables
{
    public class TblBatchPRHdr
    {
        public string BatchPRNo { get; set; }

        public DateTime RequestDate { get; set; }

        public int TotalRows { get; set; }

        public decimal TotalAmount { get; set; }

        public string FileName { get; set; }

        public string TrxMonth { get; set; }

        public int TrxWeek { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public bool UploadStat { get; set; }

        public string EditUser { get; set; }

        public DateTime EditDate { get; set; }

    }
}
