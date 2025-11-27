using OSP.SPASv2.Web.APIServices;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public interface IRepositoryUnit 
    {
       


        VendorRepository vendorRepository { get; }
        CompanyRepository companyRepository { get; }
        BranchRepository branchRepository { get; }
        ChapelRepository chapelrepository { get; }
        PaymentRequestRepository paymentRequestRepository { get; }
        ReftrxweekRepository reftrxweekrepository { get; }
        RefDiscountRepository refdiscountrepository { get; }
    }
}
