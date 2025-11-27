using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.References
{
    public class RefChapel
    {
        public RefChapel()
        {
            ChapelCode = string.Empty;
            ChapelDesc = string.Empty;
            CompanyCode = string.Empty;
            RegionCode = string.Empty;
            TerritoryCode = string.Empty;
            ChapelClass = string.Empty;
            ChapelTypeCode = string.Empty;
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
        public string ChapelCode { get; set; }

        [StringLength(50)]
        public string ChapelDesc { get; set; }

      
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [StringLength(10)]
        public string RegionCode { get; set; }

        [StringLength(10)]
        public string TerritoryCode { get; set; }

        [StringLength(10)]
        public string ChapelClass { get; set; }

        [StringLength(10)]
        public string ChapelTypeCode { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(25)]
        public string PersonCode { get; set; }

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
