using OSP.Common.Domain.References;
using OSP.SPASv2.Repository.IRepository;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class RefBranchRepository : IRefBranchRepository<RefBranch>
    {
        public Task<IList<RefBranch>> GetBranches(string branchdesc, string company)
        {
            throw new NotImplementedException();
        }

        public Task<IList<RefBranch>> GetBranchlist()
        {
            throw new NotImplementedException();
        }
    }
}
