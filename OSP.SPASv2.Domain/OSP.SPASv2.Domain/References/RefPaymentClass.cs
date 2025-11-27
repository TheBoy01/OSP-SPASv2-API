using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class RefPaymentClass
    {
        public RefPaymentClass()
        {
            PayClassCode = string.Empty;
            PayDesc = string.Empty;
            Active = false;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            ReqDesc = string.Empty;
            HasPO = false;
            GeneralClass = string.Empty;
        }

        [Key]
        [StringLength(10)]
        public string PayClassCode { get; set; }

        [StringLength(50)]
        public string PayDesc { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [StringLength(25)]
        public string AuditUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [StringLength(50)]
        public string ReqDesc { get; set; }
        public bool HasPO { get; set; }


        [StringLength(50)]
        public string GeneralClass  { get; set; }

    }
}
