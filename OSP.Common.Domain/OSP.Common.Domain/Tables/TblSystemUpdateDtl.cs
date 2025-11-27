using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.Tables
{
    public class TblSystemUpdateDtl
    {
        [Required]
        public string UpdateCode { get; set; }
        [Required]
        public string DepartmentCode { get; set; }
        public DateTime AuditDate { get; set; }
        public string AuditUser { get; set; }
        public string Remarks { get; set; }
        public byte isUpdated { get; set; }
        public DateTime DepartmentUpdateDate { get; set; }
    }
}
