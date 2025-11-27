using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblPaymentRequestAuth
    {
        public TblPaymentRequestAuth()
        {
            Reqno = string.Empty;
            PersonID = string.Empty;
            StatusType = string.Empty;
            IsRead = false;
            ReadDate = DateTime.Now;
            AuthorizedDate = DateTime.Now;
            Remarks = string.Empty;
            AuthorizeLevel = 0;
            AuthorizeClass = string.Empty;
           
        }
        [Key]
        [Required]
        [StringLength(30)]
        public string Reqno { get; set; }


        [StringLength(25)]
        public string PersonID { get; set; }

        [StringLength(5)]
        public string StatusType { get; set; }

        public bool IsRead { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReadDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuthorizedDate { get; set; }

        [StringLength(300)]
        public string Remarks { get; set; }

        public int AuthorizeLevel { get; set; }

        public string AuthorizeClass { get; set; }

    }
}
