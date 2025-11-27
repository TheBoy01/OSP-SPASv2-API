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
    public class RefProvinceRepository : IRefProvinceRepository<RefProvince>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefProvince> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion


        #region Contructor
        public RefProvinceRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefProvince>(_context);
        }
        #endregion

        public async Task<IList<RefProvince>> GetProvinceList()
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<RefProvince> vlist = await _context.RefProvince.FromSqlRaw("select * from RefProvince where Active=1 order by ProvinceCode Asc").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
