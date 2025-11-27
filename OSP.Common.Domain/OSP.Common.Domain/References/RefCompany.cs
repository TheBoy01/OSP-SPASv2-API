using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.References 
{
    public class RefCompany
    {
        public RefCompany()
        {
            CompanyCode = string.Empty;
            CompanyDesc = string.Empty;
            TIN = string.Empty;
            CompanyType = string.Empty;
            Active = false;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [StringLength(100)]
        public string CompanyDesc { get; set; }

        [StringLength(50)]
        public string TIN { get; set; }

        [StringLength(10)]
        public string CompanyType { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [StringLength(30)]
        public string AuditUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
