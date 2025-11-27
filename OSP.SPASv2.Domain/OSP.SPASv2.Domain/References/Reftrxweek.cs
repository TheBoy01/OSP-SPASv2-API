using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class RefTrxweek
    {
        public RefTrxweek()
        {
            TrxMonth = string.Empty;
            WeekNo = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            UploadStat = false;
        }
        [Key]
        [Required]
        [StringLength(10)]
        public string TrxMonth { get; set; }

        [Key]
        [Required]
        public int WeekNo { get; set; }

        [Required]
        //[DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        //[DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required]
        public bool UploadStat { get; set; }

    }
}
