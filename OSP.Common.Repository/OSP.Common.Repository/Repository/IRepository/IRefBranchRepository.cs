using OSP.Common.Domain.References;


namespace OSP.Common.Repository.IRepository
{
    public interface IRefBranchRepository<TEntity> where TEntity : class
    {
      
        public Task<IList<RefBranch>> GetBranchlist();
        public Task<IList<RefBranch>> GetBranches(string branchdesc, string company);
        public Task<IList<RefBranch>> GetBranchesByPersonID(string personid);
        public Task<qryBranch> GetBranchdetails(string companycode, string branchcode);
    }
}
