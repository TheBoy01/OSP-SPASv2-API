using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.Tables
{
    public class TblResponse
    {
        public TblResponse()
        {
            TrxNo = string.Empty;
            UniqueInfo = string.Empty;
            Status = string.Empty;
            ErrorMessage = string.Empty;
            MethodName = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [StringLength(30)]
        public string TrxNo { get; set; }

        [StringLength(30)]
        public string UniqueInfo { get; set; }

        [StringLength(8)]
        public string Status { get; set; }

        [StringLength(300)]
        public string ErrorMessage { get; set; }

        [StringLength(30)]
        public string MethodName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }
    }
}

