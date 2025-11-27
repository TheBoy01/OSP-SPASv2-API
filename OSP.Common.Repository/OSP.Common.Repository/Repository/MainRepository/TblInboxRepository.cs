using Common.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.IRepository;
using OSP.Common.Repository.IRepository.Table;
using OSP.Common.Repository.Repository.IRepository;
using System.Data;
using System.Text;

namespace OSP.Common.Repository.Repository.MainRepository
{
    public class TblInboxRepository : ITblInboxRepository<TblInbox>
    {

        private OSPContext _context;
        AbstractRepository<TblInbox> _AbstractRepository;
        StringBuilder sb;

        public TblInboxRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPIS2");
            _AbstractRepository = new AbstractRepository<TblInbox>(_context);
            sb = new StringBuilder();
        }



    }
}
