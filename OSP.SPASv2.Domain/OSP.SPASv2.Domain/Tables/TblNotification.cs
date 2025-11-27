using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblNotification
    {
        public TblNotification()
        {
            Idx = 0;
            SystemCode = string.Empty;
            ReferenceCode = string.Empty;
            ReferenceNo = string.Empty;
            NotificationCode = string.Empty;
            Sender = string.Empty;
            Receiver = string.Empty;
            SendType = string.Empty;
            StatusCode = string.Empty;
            Remarks = string.Empty;
            SendDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        public int Idx { get; set; }

        [Required]
        [StringLength(10)]
        public string SystemCode { get; set; }

        [Required]
        [StringLength(10)]
        public string ReferenceCode { get; set; }

        [Required]
        [StringLength(20)]
        public string ReferenceNo { get; set; }

        [Required]
        [StringLength(10)]
        public string NotificationCode { get; set; }

        [Required]
        [StringLength(30)]
        public string Sender { get; set; }

        [Required]
        [StringLength(30)]
        public string Receiver { get; set; }

        [Required]
        [StringLength(10)]
        public string SendType { get; set; }

        [Required]
        [StringLength(10)]
        public string StatusCode { get; set; }

        [Required]
        [StringLength(150)]
        public string Remarks { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SendDate { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
