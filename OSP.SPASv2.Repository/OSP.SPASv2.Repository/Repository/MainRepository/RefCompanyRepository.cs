using OSP.Common.Domain.References;
using OSP.SPASv2.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefCompanyRepository : IRefCompanyRepository<RefCompany>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefCompany> _AbstractRepository;
        StringBuilder sb;
//        VendorRules vrules = new VendorRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public RefCompanyRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefCompany>(_context);
        }

        public async Task<IList<RefCompany>> GetCompanylist(string company)
        {
            try
            {
                IList<RefCompany> vlist = await _context.RefCompany.FromSqlRaw("select * from RefCompany where active = 1 and companydesc like '%"+ company +"%'").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<RefCompany>> GetCompanylist1()
        {
            try
            {
                IList<RefCompany> vlist = await _context.RefCompany.FromSqlRaw("select * from RefCompany where active=1 ").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<RefCompany>> GetCompanies()
        {
            try
            {
                IList<RefCompany> vlist = await  _context.RefCompany.FromSqlRaw("select * from RefCompany where active=1").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetCompanyDesc(string companycode)
        {
            try
            {
                //string vlist = await _context.RefCompany.FromSqlRaw("select companycode from RefCompany where active=1 and companydesc like '"+ company + "'").FirstOrDefaultAsync();
                var desc = await Task.FromResult(_context.RefCompany.Where(p => p.CompanyDesc.Contains(companycode))
                                                                   .Select(p => p.CompanyDesc).FirstOrDefault());

                return desc;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetCompanycode(string companydesc)
        {
            try
            {
                //string vlist = await _context.RefCompany.FromSqlRaw("select companycode from RefCompany where active=1 and companydesc like '"+ company + "'").FirstOrDefaultAsync();
                var companycode = await  Task.FromResult(_context.RefCompany.Where(p => p.CompanyDesc.Contains(companydesc))
                                                                   .Select(p=>p.CompanyCode).FirstOrDefault());

                return companycode;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryCompanyType>> GetCompanyTypes(string company)
        {
            try
            {
                IList<qryCompanyType> vlist = await _context.qryCompanyType.FromSqlRaw("select distinct companytype from RefCompany where active = 1 and companytype like '%" + company + "%'").ToListAsync();
                

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryCompanyType>> GetCompanyTypesAccess(string personid)
        {
            try
            {
                IList<qryCompanyType> vlist = await _context.qryCompanyType.FromSqlRaw("select distinct companytype from RefCompany where active = 1 and companytype  in (select companytype from tblpersonaccess where personid ='"+ personid +"')").ToListAsync();


                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetCompanyDescByCompanyCode(string companyCode)
        {
            try
            {
                return await Task.FromResult(_context.RefCompany.Where(p => p.CompanyCode.Equals(companyCode))
                                                                .Select(p => p.CompanyDesc).FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion

        #region Public Functions







        #endregion
    }
}
