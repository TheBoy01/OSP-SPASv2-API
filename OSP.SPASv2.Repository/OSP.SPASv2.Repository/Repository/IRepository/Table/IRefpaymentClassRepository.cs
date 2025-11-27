using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;


namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefpaymentClassRepository<TEntity> where TEntity : class
    {

        public Task<string> GetGetPayclassCodeByDesc(string PayClassCode);

        public Task<RefPaymentClass> Read(string PayClassCode);

    }
}
