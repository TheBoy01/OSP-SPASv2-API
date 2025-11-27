using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblDataSourceDtl
    { 
        public int Idx { get; set; }

        public string ReferenceNo { get; set; }

        public string AccountCode { get; set; }

        public string BranchAcctCode { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Debit { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N4}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(13,4)")]
        public decimal Credit { get; set; }

        public string Note { get; set; }

    }
}
