using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    //[DataContract]
    public class TblVendorItems
    {
        public string VendorCode { get; set; }

        public string ItemCode { get; set; }

        public string ItemDesc { get; set; }

        public string Category { get; set; }

        public string UOM { get; set; }

        public string PaymentClassCode { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsDefault { get; set; }

        public bool Active { get; set; }

        public string AuditUser { get; set; }

        public DateTime AuditDate { get; set; }

        public string CompanyType { get; set; }

        public decimal Amount { get; set; }

    }

}
