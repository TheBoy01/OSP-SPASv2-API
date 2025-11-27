using OSP.Common.Domain.Params;

namespace OSP.Common.Repository.Repository.IRepository
{
    public interface ITblOutboxRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> SaveToOutbox(InfoTextParams InfoTextParams);
    }
}
