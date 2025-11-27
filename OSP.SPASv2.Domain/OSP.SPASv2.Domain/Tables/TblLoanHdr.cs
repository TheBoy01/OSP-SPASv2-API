using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblLoanhdr
    {
        public TblLoanhdr()
        {
            LAFNo = string.Empty;
            LPANo = string.Empty;
            //RefCode = 0;
            //BranchApplied = string.Empty;
            //ReleaseDate = DateTime.Now;
            //LoanFPDate = DateTime.Now;
            //DateApplied = DateTime.Now;
            AppliedLoan = 0.00M;
            //ApprovedLoan = 0.00M;
            //NetProceeds = 0.00M;
            //LoanTerm = 0;
            //TotalAmountPaid = 0.00M;
            //LoanStat = string.Empty;
            //Remarks = string.Empty;
            //Balance = 0.00M;
            //DocSent = 0;
            //DocRecieve = 0;
            //AuditUSer = string.Empty;
            //AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(30)]
        public string LAFNo { get; set; }

        [StringLength(30)]
        public string LPANo { get; set; }

        //public int RefCode { get; set; }

        //[StringLength(6)]
        //public string BranchApplied { get; set; }

        //[DataType(DataType.DateTime)]
        //public DateTime ReleaseDate { get; set; }

        //[DataType(DataType.DateTime)]
        //public DateTime LoanFPDate { get; set; }

        //[DataType(DataType.DateTime)]
        //public DateTime DateApplied { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal AppliedLoan { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        //[DataType(DataType.Currency)]
        //public decimal ApprovedLoan { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        //[DataType(DataType.Currency)]
        //public decimal NetProceeds { get; set; }

        //public int LoanTerm { get; set; }

        //[Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        //[DataType(DataType.Currency)]
        //public decimal TotalAmountPaid { get; set; }

        //[StringLength(2)]
        //public string LoanStat { get; set; }

        //[StringLength(500)]
        //public string Remarks { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        //[DataType(DataType.Currency)]
        //public decimal Balance { get; set; }

        //public int DocSent { get; set; }

        //public int DocRecieve { get; set; }

        //[StringLength(25)]
        //public string AuditUSer { get; set; }

        //[DataType(DataType.DateTime)]
        //public DateTime AuditDate { get; set; }

    }
}
