using OSP.Common.Service.Service;
using OSP.Common.Service.Services;

namespace OSP.Common.Service.OperationContract
{
    public interface IServiceUnit
    {
        SendEmailService SendEmailService { get; set; }

        CommonService CommonService { get; set; }

        DownloadFileService DownloadFileService { get; set; }
    }
}
