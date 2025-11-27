namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefChapelBranchRepository<TEntity> where TEntity : class
    {
        public Task<List<RefChapelBranch>> GetRefChapelBranches();

    }
}
