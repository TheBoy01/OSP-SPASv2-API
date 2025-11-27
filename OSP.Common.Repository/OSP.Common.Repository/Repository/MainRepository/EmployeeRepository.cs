using Common.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.IRepository;
using OSP.Common.Repository.IRepository.Table;
using System.Data;
using System.Text;

namespace OSP.Common.Repository.Repository.MainRepository
{
    public class EmployeeRepository : IEmployeeRepository<TblEmployee>
    {

        private OSPContext _context;
        AbstractRepository<TblEmployee> _AbstractRepository;
        StringBuilder sb;

        public EmployeeRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            _AbstractRepository = new AbstractRepository<TblEmployee>(_context);
            sb = new StringBuilder();
        }

        public async Task<qryEmployee> GetEmployeeDetails(string personid)
        {
            try
            {
                //qryEmployee _qryEmployee = await _context.qryEmployee.FromSqlRaw("Select * from qryEmployee where personid = '"+ personid + "'").FirstAsync();
                
                var _qryEmployee = await _context.qryEmployee.Where(t=>t.PersonID == personid).FirstOrDefaultAsync();
                return _qryEmployee;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<IList<qryEmployee>> GetEmployeeList(IList<string> personid)
        {
            try
            {
                //qryEmployee _qryEmployee = await _context.qryEmployee.FromSqlRaw("Select * from qryEmployee where personid = '"+ personid + "'").FirstAsync();

                var _qryEmployee = new qryEmployee();
                IList<qryEmployee> list = new List<qryEmployee>();
                foreach (var item in personid)
                {
                    _qryEmployee = await _context.qryEmployee.Where(t => t.PersonID == item).FirstOrDefaultAsync();

                    list.Add(_qryEmployee);
                }
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<TblEmployee> ReadTblEmployee(string personid)
        {
            throw new NotImplementedException();
        }
    }
}
