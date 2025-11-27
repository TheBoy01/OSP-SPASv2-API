using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSP.SPASv2.Domain.View
{
    public class qryCompany
    {
        [Key]
        public string CompanyId { get; set; }
        public string? CompanyDesc { get; set; }
    }
}
