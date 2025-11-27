using OSP.Common.Domain.Tables;
using OSP.Common.Domain.View;

namespace OSP.Common.Domain.Params
{
    public class OSPParams
    {
        public IList<TblRecipient> TblRecipientList { get; set; }
        public IList<qryEmployee> qryEmployeeList { get; set; }
        public IList<string> PersonIdList { get; set; }
        public IList<string> listNo { get; set; }
        public TblResponse TblResponse { get; set; }    
        public IList<TblNotification> TblNotificationList { get; set;}
        public IList<TblSendemaildtl> tblSendemaildtlsList { get; set; }
        public IList<TblSmtp> tblSmtpList { get; set; }
    }

}
