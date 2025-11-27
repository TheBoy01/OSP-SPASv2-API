using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class RefBank
    {
        public RefBank()
        {
            BankCode = string.Empty;
            BankName = string.Empty;
            Active = false;
            Startdate = DateTime.Now;
            Enddate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(10)]
        public string BankCode { get; set; }

        [Required]
        [StringLength(75)]
        public string BankName { get; set; }

        [Required]
        public bool Active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Startdate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Enddate { get; set; }

        [Required]
        [StringLength(25)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
