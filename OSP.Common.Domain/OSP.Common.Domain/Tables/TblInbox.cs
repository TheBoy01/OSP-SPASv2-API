using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.Tables
{
    public class TblInbox
    { 
        public TblInbox()
        {
            MsgID = 0;
            Msg = string.Empty;
            MPN = string.Empty;
            DateStamp = DateTime.Now;
            Status = 0;
            COMNum = 0;
            smsID = 0;
            Page = 0;
            Pages = 0;
        }
        public int MsgID { get; set; }

        [StringLength(450)]
        public string Msg { get; set; }

        [StringLength(20)]
        public string MPN { get; set; }

        public DateTime DateStamp { get; set; }

        public byte Status { get; set; }

        public byte COMNum { get; set; }

        public byte smsID { get; set; }

        public byte Page { get; set; }

        public byte Pages { get; set; }

    }
}
