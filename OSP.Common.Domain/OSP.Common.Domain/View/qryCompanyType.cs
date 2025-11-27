using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.View 
{
    public class qryCompanyType
    {
        public qryCompanyType()
        {
            
            CompanyType = string.Empty;
            
        }
        [Key]
        public string CompanyType { get; set; }

        
    }
}
