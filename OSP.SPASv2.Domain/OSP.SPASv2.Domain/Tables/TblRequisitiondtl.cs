using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblRequisitiondtl
    {
        public TblRequisitiondtl()
        {
            ReqItemNo = 0;
            ReqNo = string.Empty;
            CompanyCode = string.Empty;
            DeptCode = string.Empty;
            ItemCode = string.Empty;
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
            Deduction = 0.00M;
            FreightPerUnit = 0.00M;
            Freight = 0.00M;
        }
        [Key]
        [Required]
        public int ReqItemNo { get; set; }

        [Required]
        [StringLength(25)]
        public string ReqNo { get; set; }

        [Required]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(10)]
        public string DeptCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ItemCode { get; set; }

        [Required]
        [StringLength(5)]
        public string Unit { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Gross { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal VatRate { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Vat { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal NetofVat { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal TotalTax { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Discount { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
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

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Deduction { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal FreightPerUnit { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Freight { get; set; }

        

    }
}
