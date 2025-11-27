using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.References
{
    public class RefBankAcctType
    {
        public string AcctTypeCode { get; set; }

        public bool Active { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
