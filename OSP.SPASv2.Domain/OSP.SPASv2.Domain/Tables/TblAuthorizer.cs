using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblAuthorizer
    {
        public TblAuthorizer()
        {
            PersonID = string.Empty;
            PayClassCode = string.Empty;
            //CompanyType = string.Empty;
            //DeptCode = string.Empty;
            //PositionCode = string.Empty;
            AuthorizedLevel = false;
            //TypeOfAuthorize = string.Empty;
            ReqType = string.Empty;
            AmountAuthorizeFrom = 0.00M;
            AmountAuthorizeTo = 0.00M;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            EditUser = string.Empty;
            EditDate = DateTime.Now;
            Active = false;
        }
        [Key]
        [Required]
        [StringLength(30)]
        public string PersonID { get; set; }

        [Key]
        [Required]
        [StringLength(10)]
        public string PayClassCode { get; set; }

        //[Key]
        //[Required]
        //[StringLength(10)]
        //public string CompanyType { get; set; }

        //[Required]
        //[StringLength(10)]
        //public string DeptCode { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string PositionCode { get; set; }

        [Required]
        public bool AuthorizedLevel { get; set; }

        [Required]
        public bool Active { get; set; }

        //[Required]
        //[StringLength(30)]
        //public string TypeOfAuthorize { get; set; }

        [Key]
        [Required]
        [StringLength(30)]
        public string ReqType { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal AmountAuthorizeFrom { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal AmountAuthorizeTo { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        [StringLength(30)]
        public string EditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditDate { get; set; }

    }
}
