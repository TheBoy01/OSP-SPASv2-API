using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.References
{
    public class RefSystems
    {
       
      
     
        public string DeptCode { get; set; }

        [Key]
        public string SystemCode { get; set; }

      
        public string SystemName { get; set; }

       
        public int MaxIdle { get; set; }

        public DateTime LastLogIn { get; set; }

       
        public string Version { get; set; }

      
        public DateTime LastSave { get; set; }

        public bool Active { get; set; }

     
        public string AuditUser { get; set; }

       
        public DateTime AuditDate { get; set; }

        public bool UploadStat { get; set; }

        public DateTime EditDate { get; set; }

       
        public DateTime VersionDate { get; set; }

    }
}

