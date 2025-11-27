namespace OSP.SPASv2.Domain.Tables
{
    public class TblBatchPRItems
    { 
        public int idx { get; set; }

        public string PRNo { get; set; }

        public string ItemCode { get; set; }

        public decimal Amount { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public string EditUser { get; set; }

        public DateTime EditDate { get; set; }

    }
}
