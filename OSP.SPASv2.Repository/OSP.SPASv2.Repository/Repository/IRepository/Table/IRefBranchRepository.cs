using OSP.Common.Domain.References;


namespace OSP.SPASv2.Repository.IRepository
{
    public interface IRefBranchRepository<TEntity> where TEntity : class
    {
      
        public Task<IList<RefBranch>> GetBranchlist();
        public Task<IList<RefBranch>> GetBranches(string branchdesc, string company);
    }
}
