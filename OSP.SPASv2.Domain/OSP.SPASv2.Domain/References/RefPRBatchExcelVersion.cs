namespace OSP.SPASv2.Domain.References
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    namespace OSP.SPASv2.Domain.Tables
    {
        public class RefPrbatchexcelversion
        {
            public RefPrbatchexcelversion()
            {
                VersionNo = string.Empty;
                VersionStartDate = Convert.ToDateTime("1/1/1900");
                EditUser = string.Empty;
                EditDate = DateTime.Now;
                AuditUser = string.Empty;
                AuditDate = DateTime.Now;
            }
            [Required]
            [StringLength(10)]
            public string VersionNo { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime VersionStartDate { get; set; }

            [Required]
            [StringLength(25)]
            public string EditUser { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            public DateTime EditDate { get; set; }

            [Required]
            [StringLength(25)]
            public string AuditUser { get; set; }

            [Required]
            [DataType(DataType.DateTime)]
            public DateTime AuditDate { get; set; }

        }
    }

}
