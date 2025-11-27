using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblDataSourceHdr
    {
        [Key]
        public string BatchName { get; set; }

        [Key]
        public string ReferenceNo { get; set; }

        public string ClassID { get; set; }

        public string VendorID { get; set; }

        public string BankCode { get; set; }

        public string BankAccountNumber { get; set; }

        public string CompanyName { get; set; }

        public string CheckName { get; set; }

        public string AccountDeptCode { get; set; }

        public string Reason { get; set; }

        public string Remarks { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal AmountDue { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal DebitAmount { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal DebitInput { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal CreditWtax { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal CreditMisc { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal CreditAP { get; set; }

        public string Email { get; set; }

        public string ContactNo { get; set; }

        public string SystemCode { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

    }

}
