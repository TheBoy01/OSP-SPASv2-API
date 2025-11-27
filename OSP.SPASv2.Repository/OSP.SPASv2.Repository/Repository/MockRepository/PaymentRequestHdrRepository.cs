using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class PaymentRequestHdrRepository : IPaymentRequestHdrRepository<TblPaymentrequesthdr>
    {
        public Task<TblResponse> CreatePaymentRequest(TblPaymentrequesthdr entity)
        {
            throw new NotImplementedException();
        }

        

        public Task<TblPaymentrequesthdr> GetLatestPRRow(string companycode, string branchcode)
        {
            throw new NotImplementedException();
        }

        public Task<qryPaymentRequestHdr> GetPaymentrequesthdr(string prno)
        {
            throw new NotImplementedException();
        }
    }
}
