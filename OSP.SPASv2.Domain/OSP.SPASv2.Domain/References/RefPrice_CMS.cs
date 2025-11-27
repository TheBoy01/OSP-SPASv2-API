using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSP.SPASv2.Domain.References
{
    public class RefPrice_CMS
    {
        public RefPrice_CMS()
        {
            FactoryCode = string.Empty;
            CasketCode = string.Empty;
            PriceCode = string.Empty;
            PriceDesc = string.Empty;
            Price = 0;
            StartEffectivityDate = Convert.ToDateTime("1/1/1900");
            EndEffectivityDate = Convert.ToDateTime("1/1/1900");
            Active = false;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            UploadStat = false;
            EditUser = string.Empty;
            EditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(50)]
        public string PriceCode { get; set; }

        [Required]
        [StringLength(10)]
        public string FactoryCode { get; set; }

        [Required]
        [StringLength(20)]
        public string CasketCode { get; set; }

        [Required]
        [StringLength(50)]
        public string PriceDesc { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Price { get; set; }

        [Required]
        public bool Active { get; set; }


        [Required]
        public bool UploadStat { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartEffectivityDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndEffectivityDate { get; set; }

        [Required]
        [StringLength(25)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        [StringLength(25)]
        public string EditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditDate { get; set; }

    }

    
}
