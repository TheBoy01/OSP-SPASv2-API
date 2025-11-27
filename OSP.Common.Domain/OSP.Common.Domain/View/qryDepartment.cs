using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.View
{
    public class qryDepartment
    {
        public qryDepartment()
        {

            DeptCode = string.Empty;
            DeptDesc = string.Empty;

        }

        [Key]
        public string DeptCode { get; set; }

        public string DeptDesc { get; set; }
    }
}
