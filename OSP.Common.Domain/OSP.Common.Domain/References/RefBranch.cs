using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.References
{
    public class RefBranch
    {
        public RefBranch()
        {
            BranchCode = string.Empty;
            BranchDesc = string.Empty;
            MotherBranchCode = string.Empty;
            CompanyCode = string.Empty;
            RegionCode = string.Empty;
            TerritoryCode = string.Empty;
            BranchClassCode = string.Empty;
            Address = string.Empty;
            PersonCode = string.Empty;
            Active = false;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(10)]
        public string BranchCode { get; set; }

        [Required]
        [StringLength(50)]
        public string BranchDesc { get; set; }

        [Required]
        [StringLength(10)]
        public string MotherBranchCode { get; set; }

        [Required]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(10)]
        public string RegionCode { get; set; }

        [Required]
        [StringLength(10)]
        public string TerritoryCode { get; set; }

        [Required]
        [StringLength(10)]
        public string BranchClassCode { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [StringLength(25)]
        public string PersonCode { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(25)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
