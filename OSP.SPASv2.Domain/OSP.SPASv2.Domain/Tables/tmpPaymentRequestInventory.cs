using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.Tables
{
    public class tmpPaymentRequestInventory
    {

        [Key]
        public string PRNo { get; set; }

        [Key]
        public string ItemCode { get; set; }

        [DisplayName("Description")]
        public string ItemDesc { get; set; }

        [DisplayName("Units")]
        public string UOM { get; set; }


        public int Qty { get; set; }

        public decimal Price { get; set; }
        public decimal Gross { get; set; }

        public decimal VatRate { get; set; }

        //public decimal GovDiscRate { get; set; }

        [DisplayName("VAT")]
        public decimal Vat { get; set; }

        [DisplayName("Net of VAT")]
        public decimal NetofVat { get; set; }

        public decimal ATC { get; set; }

        public decimal Discount { get; set; }

        [DisplayName("Total")]
        public decimal TotalAmt { get; set; }

        public string AuditUser { get; set; }


    }
}