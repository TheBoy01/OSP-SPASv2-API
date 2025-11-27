using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OSP.SPASv2.Domain.Tables
{
    //[Keyless]
    public class DummyStr
    {
        //[Key]
        public string str { get; set; }
    }
}
