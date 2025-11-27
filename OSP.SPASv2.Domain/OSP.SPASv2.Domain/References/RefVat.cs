using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class RefVat
    {
        public RefVat()
        {
            Vatcode = string.Empty;
            Vat = 0.00M;
            Active = false;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            AuditUser = string.Empty;
            AuditDate = DateTime.Now;
        }
        [Key]
        [StringLength(5)]
        public string Vatcode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}", ConvertEmptyStringToNull = true)]
        [DataType(DataType.Currency)]
        public decimal Vat { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [StringLength(25)]
        public string AuditUser { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AuditDate { get; set; }

    }
}
