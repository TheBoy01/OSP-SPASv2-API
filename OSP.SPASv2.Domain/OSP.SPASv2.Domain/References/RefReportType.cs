using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.References
{
    public class RefReportType
    {
        public RefReportType()
        {
            ReportID = string.Empty;
            ReportType = string.Empty;
            Active = false;
            seqno = 0;
        }
        [Key]
        [Required]
        [StringLength(10)]
        public string ReportID { get; set; }

        [Required]
        [StringLength(50)]
        public string ReportType { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int seqno { get; set; }

    }
}
