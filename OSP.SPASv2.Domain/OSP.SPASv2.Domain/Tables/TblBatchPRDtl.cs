namespace OSP.SPASv2.Domain.Tables
{
    public class TblBatchPRDtl
    {
        public string BatchPRNo { get; set; }

        public string PRNo { get; set; }

        public string ExcelReqIdx { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public bool UploadStat { get; set; }

        public string EditUser { get; set; }

        public DateTime EditDate { get; set; }

    }
}
