using Microsoft.EntityFrameworkCore;

namespace OSP.SPASv2.Domain.View
{
   
    public class qryPaymentRequestAuthDtl
    {
      
        public string PRNo { get; set; }

        public string AuthorizeClass { get; set; }

        public string PersonID { get; set; }

        public string Name { get; set; }


        public int AuthorizeLevel { get; set; }

        public string Emailaddress { get; set; }

        public string ContactNo { get; set; }

        public string Remarks { get; set; }

        public DateTime AuthorizedDate { get; set; }
    }
}
