using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.References
{
    public class RefDepartments
    {
        public RefDepartments()
        {
            DeptCode = string.Empty;
            DeptDesc = string.Empty;
            CompanyCode = string.Empty;
            DivisionCode = string.Empty;
            TerritoryCode = string.Empty;
            DeptHead = string.Empty;
            StartDate = Convert.ToDateTime("1/1/1900");
            EndDate = Convert.ToDateTime("1/1/1900");
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            EditUser = string.Empty;
            EditDate = DateTime.Now;
            DeptType = string.Empty;
            DeptClass = string.Empty;
        }
        [Key]
        [Required]
        [StringLength(10)]
        public string DeptCode { get; set; }

        [Required]
        [StringLength(75)]
        public string DeptDesc { get; set; }

        [Key]
        [Required]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(10)]
        public string DivisionCode { get; set; }

        [Required]
        [StringLength(10)]
        public string TerritoryCode { get; set; }

        [Required]
        [StringLength(50)]
        public string DeptHead { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        [StringLength(30)]
        public string EditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditDate { get; set; }

        [StringLength(5)]
        public string DeptType { get; set; }

        [StringLength(5)]
        public string DeptClass { get; set; }

    }
}
