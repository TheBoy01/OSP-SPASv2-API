using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblPaymentrequestotherdetails
    {
        public TblPaymentrequestotherdetails()
        {
            PRNo = string.Empty;
            BillPeriod = string.Empty;
            DocNo = string.Empty;
            PONo = string.Empty;
            DRNo = string.Empty;
            DRDate = Convert.ToDateTime("1/1/1900");
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(25)]
        public string PRNo { get; set; }

        [StringLength(50)]
        public string BillPeriod { get; set; }

        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(50)]
        public string PONo { get; set; }

        [StringLength(50)]
        public string DRNo { get; set; }

        [DataType(DataType.Date)]
        public DateTime DRDate { get; set; }

        [StringLength(50)]
        public string AuditUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
