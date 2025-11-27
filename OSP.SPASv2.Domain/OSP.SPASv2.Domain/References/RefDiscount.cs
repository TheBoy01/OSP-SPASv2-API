using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class RefDiscount
    {
        public RefDiscount()
        {
            DiscountCode = string.Empty;
            DiscountDesc = string.Empty;
            Active = false;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [StringLength(10)]
        public string DiscountCode { get; set; }

        [StringLength(10)]
        public string DiscountDesc { get; set; }

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
