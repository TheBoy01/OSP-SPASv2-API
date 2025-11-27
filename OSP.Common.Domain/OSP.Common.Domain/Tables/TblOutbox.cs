using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.Tables
{
    public class TblOutbox
    {
        public TblOutbox()
        {
            MsgID = 0;
            Msg = string.Empty;
            MPN = string.Empty;
            Datestamp = DateTime.Now;
            Status = 0;
            Priority = 0;
            UserID = string.Empty;
            COMNum = 0;
        }

        public int MsgID { get; set; }

        [StringLength(450)]
        public string Msg { get; set; }

        [StringLength(20)]
        public string MPN { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Datestamp { get; set; }

        public byte Status { get; set; }

        public byte Priority { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public byte COMNum { get; set; }
    }
}
