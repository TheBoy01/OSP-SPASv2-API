using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class RptPurchaseorder
    {
        public RptPurchaseorder()
        {
            CompanyDesc = string.Empty;
            Address = string.Empty;
            PONo = string.Empty;
            ReqNo = string.Empty;
            VendorName = string.Empty;
            PayeeName = string.Empty;
            TIN = string.Empty;
            PayMethod = string.Empty;
            PayClass = string.Empty;
            Department = string.Empty;
            Description = string.Empty;
            Qty = 0;
            UOM = string.Empty;
            UnitPrice = 0.00M;
            TotalPrice = 0.00M;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            Terms = string.Empty;
            isPDF = false;
            BatchNo = string.Empty;
            Deduction = 0.00M;
            Freight = 0.00M;
            VAT = 0.00M;
            NetofVAT = 0.00M;
            TotalTax = 0.00M;
            TotalAmount = 0.00M;

        }
        [Required]
        [StringLength(200)]
        public string CompanyDesc { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Key]
        [Required]
        [StringLength(25)]
        public string PONo { get; set; }

        [Required]
        [StringLength(25)]
        public string ReqNo { get; set; }

        [Required]
        [StringLength(100)]
        public string VendorName { get; set; }

        [Required]
        [StringLength(100)]
        public string PayeeName { get; set; }

        [Required]
        [StringLength(50)]
        public string TIN { get; set; }

        [Required]
        [StringLength(50)]
        public string PayMethod { get; set; }

        [Required]
        [StringLength(50)]
        public string PayClass { get; set; }

        [Key]
        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        [Key]
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        [StringLength(10)]
        public string UOM { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [StringLength(10)]
        public string Terms { get; set; }

        public bool isPDF { get; set; }

        [StringLength(25)]
        public string BatchNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Deduction { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Freight { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal VAT { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal NetofVAT { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal TotalTax { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Discount { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal TotalAmount { get; set; }

    }
}
