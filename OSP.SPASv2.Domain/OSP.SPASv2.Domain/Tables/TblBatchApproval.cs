using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblBatchApproval
    {
        public TblBatchApproval()
        {
            BANo = string.Empty;
            ReqNo = string.Empty;
            ReqType = string.Empty;
            Active = false;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(20)]
        public string BANo { get; set; }

        [Key]
        [Required]
        [StringLength(20)]
        public string ReqNo { get; set; }

        [Required]
        [StringLength(2)]
        public string ReqType { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
