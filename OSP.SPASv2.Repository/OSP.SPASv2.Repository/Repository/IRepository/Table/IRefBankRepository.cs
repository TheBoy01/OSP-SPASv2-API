using OSP.Common.Domain.References;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefBankRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefBank>> GetBankList();

    }
}
