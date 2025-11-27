using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblCasketorderhdr
    {
        public TblCasketorderhdr()
        {
            FactoryCode = string.Empty;
            PONo = string.Empty;
            ChapelCode = string.Empty;
            CompanyCode = string.Empty;
            PODate = Convert.ToDateTime("1/1/1900");
            POReceivedDate = DateTime.Now;
            Terms = 0;
            Remarks = string.Empty;
            POAmount = 0.00M;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            UploadStat = false;
            EditUser = string.Empty;
            EditDate = DateTime.Now;
            Void = false;
            VoidUser = string.Empty;
            VoidDate = DateTime.Now;
            SONo = string.Empty;
        }
        [Required]
        [StringLength(6)]
        public string FactoryCode { get; set; }

        [Key]
        [Required]
        [StringLength(25)]
        public string PONo { get; set; }

        [Required]
        [StringLength(6)]
        public string ChapelCode { get; set; }

        [Required]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PODate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime POReceivedDate { get; set; }

        [Required]
        public int Terms { get; set; }

        [Required]
        [StringLength(150)]
        public string Remarks { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal POAmount { get; set; }

        [Required]
        [StringLength(25)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        public bool UploadStat { get; set; }

        [Required]
        [StringLength(25)]
        public string EditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditDate { get; set; }

        [Required]
        public bool Void { get; set; }

        [Required]
        [StringLength(25)]
        public string VoidUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime VoidDate { get; set; }

        [StringLength(25)]
        public string SONo { get; set; }

    }
}
