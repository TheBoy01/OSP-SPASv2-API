using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class RefItems
    {
        public RefItems()
        {
            ItemCode = string.Empty;
            SKU = string.Empty;
            ItemDesc = string.Empty;
            UOMCode = string.Empty;
            PayClassCode = string.Empty;
            CategoryID = string.Empty;
            Size = string.Empty;
            Color = string.Empty;
            Active = false;
            StartDate = Convert.ToDateTime("1/1/1900");
            EndDate = Convert.ToDateTime("1/1/1900");
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(50)]
        public string ItemCode { get; set; }

        [Required]
        [StringLength(20)]
        public string SKU { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemDesc { get; set; }

        [Required]
        [StringLength(5)]
        public string UOMCode { get; set; }

        [Required]
        [StringLength(10)]
        public string PayClassCode { get; set; }

        [StringLength(4)]
        public string CategoryID { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(20)]
        public string Color { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(25)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
