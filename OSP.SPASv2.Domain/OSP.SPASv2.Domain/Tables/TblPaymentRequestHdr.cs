using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblPaymentrequesthdr
    {
        public TblPaymentrequesthdr()
        {
            PRNo = string.Empty;
            CompanyCode = string.Empty;
            DeptCode = string.Empty;
            RequestDate = DateTime.Now;
            PayClassCode = string.Empty;
            Active = false;
            VendorCode = string.Empty;
            PayeeName = string.Empty;
            PayMethodType = string.Empty;
            BankCode = string.Empty;
            Destination = string.Empty;
            TotalAmount = 0.00M;
            Remarks = string.Empty;
            Void = false;
            VoidUser = string.Empty;
            VoidDate = DateTime.Now;
            Printed = false;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            UploadStat = false;
            EditUser = string.Empty;
            EditDate = DateTime.Now;
            TrxMonth = string.Empty;
            TrxWeek = 0;
            RefNo = string.Empty;
        }
        [Key]
        [Required]
        [StringLength(25)]
        public string PRNo { get; set; }

        [Required]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [Required]
        [StringLength(10)]
        public string DeptCode { get; set; }

        [Required]
        //[DataType(DataType.DateTime)]
        public DateTime RequestDate { get; set; }

        [Required]
        [StringLength(10)]
        public string PayClassCode { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [StringLength(20)]
        public string VendorCode { get; set; }

        [Required]
        [StringLength(100)]
        public string PayeeName { get; set; }

        [Required]
        [StringLength(20)]
        public string PayMethodType { get; set; }

        [Required]
        [StringLength(10)]
        public string BankCode { get; set; }

        [Required]
        [StringLength(30)]
        public string Destination { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(200)]
        public string Remarks { get; set; }

        [Required]
        public bool Void { get; set; }

       // [Required]
        [StringLength(30)]
        public string VoidUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime VoidDate { get; set; }

        [Required]
        public bool Printed { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        //[DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        public bool UploadStat { get; set; }

        [Required]
        [StringLength(30)]
        public string EditUser { get; set; }

        [Required]
       // [DataType(DataType.DateTime)]
        public DateTime EditDate { get; set; }

        [Required]
        [StringLength(5)]
        public string TrxMonth { get; set; }

        [Required]
        public int TrxWeek { get; set; }

        [Required]
        public string RefNo { get; set; }

    }
}
