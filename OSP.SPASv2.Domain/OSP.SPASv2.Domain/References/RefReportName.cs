using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.References
{
    public class RefReportname
    {
        public RefReportname()
        {
            ReportNameID = string.Empty;
            ReportName = string.Empty;
            ReportID = string.Empty;
            Active = false;
            seqno = 0;
            FileType = string.Empty;
            isAudit = false;
        }
        [Key]
        [Required]
        [StringLength(25)]
        public string ReportNameID { get; set; }

        [Required]
        [StringLength(50)]
        public string ReportName { get; set; }

        [Required]
        [StringLength(10)]
        public string ReportID { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int seqno { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; }

        [Required]
        public bool isAudit { get; set; }

    }
}
