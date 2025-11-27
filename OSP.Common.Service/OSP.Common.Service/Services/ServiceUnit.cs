using OSP.Common.Service.OperationContract;
using OSP.Common.Service.Services;

namespace OSP.Common.Service.Service
{
    public class ServiceUnit
    {

        private SendEmailService _SendEmailService ;
        private SendSMSService _SendSMSService;
        private CommonService _CommonService;

        private DownloadFileService _DownloadFileService ;

        public DownloadFileService DownloadFileService
        {
            get
            {
                if (_DownloadFileService == null)
                {
                    this._DownloadFileService = new DownloadFileService();
                }
                return _DownloadFileService;
            }
        }

        public CommonService CommonService
        {
            get
            {
                if (_CommonService == null)
                {
                    this._CommonService = new CommonService();
                }
                return _CommonService;
            }
        }

        public SendEmailService SendEmailService
        {
            get
            {
                if (_SendEmailService == null)
                {
                    this._SendEmailService = new SendEmailService();
                }
                return _SendEmailService;
            }
        }

        public SendSMSService SendSMSService
        {
            get
            {
                if (_SendSMSService == null)
                {
                    this._SendSMSService = new SendSMSService();
                }
                return _SendSMSService;
            }
        }

    }
}
