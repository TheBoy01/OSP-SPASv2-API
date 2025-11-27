using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblPurchaseorderhdr
    {
        public TblPurchaseorderhdr()
        {
            PONo = string.Empty;
            Reqno = string.Empty;
            PODate = DateTime.Now;
            Active = false;
            Remarks = string.Empty;
            Printed = false;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            TrxMonth = string.Empty;
            TrxWeek = 0;
        }
        [Key]
        [Required]
        [StringLength(25)]
        public string PONo { get; set; }

        [Required]
        [StringLength(25)]
        public string Reqno { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PODate { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [StringLength(200)]
        public string Remarks { get; set; }

        [Required]
        public bool Printed { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        [StringLength(5)]
        public string TrxMonth { get; set; }

        [Required]
        public int TrxWeek { get; set; }

    }
}
