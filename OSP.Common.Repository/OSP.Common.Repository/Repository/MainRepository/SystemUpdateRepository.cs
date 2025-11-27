using Common.Repository.Repository;
using OSP.Common.Domain;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.IRepository;


namespace OSP.Common.Repository.Repository.MainRepository
{
    public class SystemUpdateRepository : IUserRepository<TblUser>
    {

        private OSPContext _context;
        AbstractRepository<TblSystemUpdateDtl> _AbstractRepository;

        public Task<TblUser> GetUserDetails(TblUser entity)
        {
            throw new NotImplementedException();
        }

        public SystemUpdateRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            _AbstractRepository = new AbstractRepository<TblSystemUpdateDtl>(_context);

        }


    }
}
