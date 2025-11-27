using OSP.SPASv2.Web.APIServices;

namespace OSP.SPASv2.Web.APIServices.Services
{
    public interface IServiceUnit
    {

        VendorService vendorService { get; }
        BranchService branchService { get; }
        CompanyService companyService { get; }
        AccountService accountService { get; }
        PaymentRequestService paymentRequestService { get; }

    }
}
