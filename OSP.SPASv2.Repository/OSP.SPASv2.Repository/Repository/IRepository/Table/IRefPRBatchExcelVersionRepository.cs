using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Domain.References.OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefPRBatchExcelVersionRepository<TEntity> where TEntity : class
    { 
        public Task<RefPrbatchexcelversion> CheckBatchVersion();


    }
}
