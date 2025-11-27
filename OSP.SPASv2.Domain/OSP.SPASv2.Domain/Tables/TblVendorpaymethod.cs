using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblVendorpaymethod
    {
        public TblVendorpaymethod()
        {
            VendorCode = string.Empty;
            PayType = string.Empty;
            BankCode = string.Empty;
            AcctNo = string.Empty;
            IsDefault = false;
            Active = false;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(10)]
        public string VendorCode { get; set; }

        [StringLength(10)]
        public string PayType { get; set; }

        [Required]
        [StringLength(10)]
        public string BankCode { get; set; }

        [Key]
        [Required]
        [StringLength(30)]
        public string AcctNo { get; set; }

        public bool IsDefault { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [StringLength(25)]
        public string AuditUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
