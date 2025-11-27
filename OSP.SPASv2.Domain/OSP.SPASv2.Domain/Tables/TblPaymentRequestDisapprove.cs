using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblPaymentrequestDisapprove
    {
        public TblPaymentrequestDisapprove()
        {
            PRNo = string.Empty;
            ReasonCode = string.Empty;
            Remarks = string.Empty;
            NewPRNo = string.Empty;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(25)]
        public string PRNo { get; set; }

        [Required]
        [StringLength(10)]
        public string ReasonCode { get; set; }

        [Required]
        [StringLength(150)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(20)]
        public string NewPRNo { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
