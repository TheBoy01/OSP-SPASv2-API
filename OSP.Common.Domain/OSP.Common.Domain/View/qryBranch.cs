using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.View
{
    public class qryBranch
    {

        [Key]
        public string Branchcode { get; set; }
        public string Branchdesc { get; set; }
        public string Address { get; set; }
        public string CompanyCode { get; set; }
    }
}
