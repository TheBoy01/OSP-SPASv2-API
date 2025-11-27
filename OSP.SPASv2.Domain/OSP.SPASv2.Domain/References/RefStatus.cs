using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.References
{
    public class RefStatus
    {

        [Key]
        public string Statuscode { get; set; }

        public string StatusDesc { get; set; }
    }
}
