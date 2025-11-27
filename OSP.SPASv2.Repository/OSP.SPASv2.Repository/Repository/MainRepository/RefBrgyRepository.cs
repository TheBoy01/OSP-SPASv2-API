using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using OSP.SPASv2.Repository.Rules;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.MainRepository;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Domain;
using OSP.Common.Domain.References;
using OSP.SPASv2.Repository.IRepository.Table;
using OSP.SPASv2.Domain.References;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefBrgyRepository : IRefBrgyRepository<RefBrgy>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefBrgy> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion 

        #region Contructor
        public RefBrgyRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefBrgy>(_context);
        }

        public async Task<IList<RefBrgy>> GetBrgyList()
        {
            try
            {
                IList<RefBrgy> vlist = await _context.RefBrgy.FromSqlRaw("select * from RefBrgy where Active=1").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<RefBrgy>> GetBrgyListByCityCode(string cityCode)
        {
            try
            {
                IList<RefBrgy> vlist = await _context.RefBrgy.FromSqlRaw("select * from RefBrgy where Active=1 and CityCode='" + cityCode +"'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}
