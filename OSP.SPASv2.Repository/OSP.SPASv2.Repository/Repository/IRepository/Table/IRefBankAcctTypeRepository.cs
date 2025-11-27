using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefBankAcctTypeRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefBankAcctType>> GetBankAcctTypeList();

    }
}
