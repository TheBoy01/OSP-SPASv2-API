using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.View
{
    public class qryListOfAuthorizerPayclass
    {
        [Key]
        public string PersonID { get; set; }
        [Key]
        public string PayClassDesc { get; set; }
        public int AuthorizeLevel { get; set; }
        public string AuthorizeClass { get; set; }

    }
}
