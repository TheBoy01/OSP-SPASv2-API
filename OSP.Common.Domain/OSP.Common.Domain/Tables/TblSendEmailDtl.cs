using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.Tables
{
    public class TblSendemaildtl
    {
        public TblSendemaildtl()
        {
            ReferenceNo = string.Empty;
            EmailName = string.Empty;
            SendTo = string.Empty;
            SendCC = string.Empty;
            SendBCC = string.Empty;
            Subject = string.Empty;
            AttachmentPath = string.Empty;
            Body = string.Empty;
            StatusType = string.Empty;
            SendDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(25)]
        public string ReferenceNo { get; set; }

        [Key]
        [Required]
        [StringLength(50)]
        public string EmailName { get; set; }

        [StringLength(100)]
        public string SendTo { get; set; }

        [StringLength(500)]
        public string SendCC { get; set; }

        [StringLength(500)]
        public string SendBCC { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(500)]
        public string AttachmentPath { get; set; }

        [StringLength(-1)]
        public string Body { get; set; }

        [StringLength(10)]
        public string StatusType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SendDate { get; set; }

        [StringLength(30)]
        public string AuditUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
