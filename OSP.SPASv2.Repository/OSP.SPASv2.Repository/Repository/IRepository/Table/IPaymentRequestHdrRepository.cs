using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IPaymentRequestHdrRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreatePaymentRequest(TEntity entity);
        public Task<TblPaymentrequesthdr> GetLatestPRRow(string companycode, string branchcode);
        //public Task<TblPaymentrequesthdr> GetPRNo(TEntity entity);
        public Task<qryPaymentRequestHdr> GetPaymentrequesthdr(string prno);
    }

}
