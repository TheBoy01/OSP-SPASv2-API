using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.Tables
{
    public class TblEmployee
    {
        public TblEmployee()
        {
            PersonID = string.Empty;
            Empcode = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            Gender = string.Empty;
            BirthDate = DateTime.Now;
            ContactNo = string.Empty;
            EmailAddress = string.Empty;
            DeptCode = string.Empty;
            PositionCode = string.Empty;
            EmpStatus = string.Empty;
            Active = false;
            Remarks = string.Empty;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            EditUser = string.Empty;
            EditDate = DateTime.Now;
        }
        [Key]
        [Required]
        [StringLength(30)]
        public string PersonID { get; set; }

        [Required]
        [StringLength(10)]
        public string Empcode { get; set; }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(75)]
        public string Address { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(15)]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(10)]
        public string DeptCode { get; set; }

        [Required]
        [StringLength(10)]
        public string PositionCode { get; set; }

        [StringLength(30)]
        public string EmpStatus { get; set; }

        [Required]
        public bool Active { get; set; }

        [StringLength(150)]
        public string Remarks { get; set; }

        [Required]
        [StringLength(25)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        [StringLength(25)]
        public string EditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EditDate { get; set; }

    }
}
