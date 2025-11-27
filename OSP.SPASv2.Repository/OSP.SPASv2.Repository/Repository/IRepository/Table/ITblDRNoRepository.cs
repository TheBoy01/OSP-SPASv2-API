using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblDRNoRepository<TEntity> where TEntity : class
    {

        public Task<TblResponse> CreateDRNo(TEntity entity);

    }
}
