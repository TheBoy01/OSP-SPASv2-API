using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.Tables
{
    public class TblRecipient
    {
        [Key]
        public int Autonumber { get; set; }
        public string SystemCode { get; set; }
        public string ReportName { get; set; }
        public string EmailType { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string AuditUser { get; set; }
        public DateTime AuditDate { get; set; }
    }
}
