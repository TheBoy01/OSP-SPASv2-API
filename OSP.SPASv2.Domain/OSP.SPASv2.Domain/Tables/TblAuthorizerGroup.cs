using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace OSP.SPASv2.Domain.Tables
{
    public class TblAuthorizerGroup
    {   [Key]
        [Required]
        public string GroupId { get; set; }
        [Key]
        [Required]
        public string PersonId { get; set; }
    }
}
