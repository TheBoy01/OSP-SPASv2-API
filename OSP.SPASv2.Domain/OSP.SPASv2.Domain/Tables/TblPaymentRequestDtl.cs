using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblPaymentrequestdtl
    {
        public TblPaymentrequestdtl()
        {
            PRProductServiceNo = 0;
            PRNo = string.Empty;
            ProductServiceCode = string.Empty;
            Unit = string.Empty;
            Price = 0.00M;
            Quantity = 0;
            Gross = 0.00M;
            VatRate = 0.00M;
            Vat = 0.00M;
            NetofVat = 0.00M;
            TotalTax = 0.00M;
            Discount = 0.00M;
            TotalAmount = 0.00M;
            Void = false;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            UploadStat = false;
            EditUser = string.Empty;
            EditDate = DateTime.Now;
        }
        [Key]
        [Required]
        public int PRProductServiceNo { get; set; }

        [Required]
        [StringLength(25)]
        public string PRNo { get; set; }

        [Required]
        [StringLength(10)]
        public string ProductServiceCode { get; set; }

        [Required]
        [StringLength(5)]
        public string Unit { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal Gross { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal VatRate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal Vat { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal NetofVat { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal TotalTax { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal Discount { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Required]
        public bool Void { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        public bool UploadStat { get; set; }

        [Required]
        [StringLength(30)]
        public string EditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditDate { get; set; }

    }
}
