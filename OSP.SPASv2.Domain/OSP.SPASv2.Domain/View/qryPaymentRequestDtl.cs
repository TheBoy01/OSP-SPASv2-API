using Microsoft.EntityFrameworkCore;

namespace OSP.SPASv2.Domain.View
{
    [Keyless]
    public class qryPaymentRequestDtl
    {

        public string PRNo { get; set; }

        public string UoM { get; set; }

        public string UoMDesc { get; set; }

        public string ItemCode { get; set; }


        public string ItemDesc { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Gross { get; set; }


        public decimal VATRate { get; set; }

        public decimal VAT { get; set; }

        public decimal LessVAT { get; set; }

        public decimal GovtDiscRate { get; set; }

        public decimal GovtDisc { get; set; }
        public decimal Net { get; set; }


        public bool Cancel { get; set; }

    }
}
