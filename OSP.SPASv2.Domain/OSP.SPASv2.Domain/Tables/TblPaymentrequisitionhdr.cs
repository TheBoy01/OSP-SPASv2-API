using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblPaymentrequisitionhdr
    {
        public TblPaymentrequisitionhdr()
        {
            Reqno = string.Empty;
            PRno = string.Empty;
            PRDate = DateTime.Now;
            Active = false;
            TotalAmount = 0.00M;
            SalesInvoiceNo = string.Empty;
            SalesInvoiceDate = DateTime.Now;
            DeliveryNo = string.Empty;
            DeliveryDate = DateTime.Now;
            Printed = false;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
            TrxMonth = string.Empty;
            TrxWeek = 0;
        }
        [Required]
        [StringLength(25)]
        public string Reqno { get; set; }

        [Key]
        [Required]
        [StringLength(25)]
        public string PRno { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PRDate { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        [StringLength(25)]
        public string SalesInvoiceNo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SalesInvoiceDate { get; set; }

        [Required]
        [StringLength(25)]
        public string DeliveryNo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public bool Printed { get; set; }

        [Required]
        [StringLength(30)]
        public string AuditUser { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

        [Required]
        [StringLength(5)]
        public string TrxMonth { get; set; }

        [Required]
        public int TrxWeek { get; set; }

    }
}
