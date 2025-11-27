using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class PaymentRequestDtlRepository : IPaymentRequestDtlRepository<TblPaymentrequestdtl>
    {
        public Task<TblResponse> CreatePaymentRequest(TblPaymentrequestdtl entity)
        {
            throw new NotImplementedException();
        }
    }
}
