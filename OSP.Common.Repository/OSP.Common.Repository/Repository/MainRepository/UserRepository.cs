using Common.Repository.Repository;
using OSP.Common.Domain;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.IRepository;

namespace OSP.Common.Repository.Repository.MainRepository
{
    public class UserRepository : IUserRepository<TblUser>
    {

        private OSPContext _context;
        AbstractRepository<TblUser> _AbstractRepository;

        public Task<TblUser> GetUserDetails(TblUser entity)
        {
            throw new NotImplementedException();
        }

        public UserRepository(OSPContext context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblUser>(_context);
        }


    }
}
