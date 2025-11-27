using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblRequisitionReason
    {
        public TblRequisitionReason()
        {
            ReqNo = string.Empty;
            ReasonCode = string.Empty;
            Remarks = string.Empty;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(25)]
        public string ReqNo { get; set; }

        [Key]
        [Required]
        [StringLength(10)]
        public string ReasonCode { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
