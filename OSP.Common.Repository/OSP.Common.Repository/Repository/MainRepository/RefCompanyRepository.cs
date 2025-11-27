using OSP.Common.Domain.References;
//using OSP.SPASv2.Repository.IRepository.Table;
//using SPASv2.Context;
//using SPASv2.Repository.Repository;
using System.Text;

using Microsoft.EntityFrameworkCore;
using OSP.Common.Repository.IRepository.Table;
using OSP.Common.Repository.Context;
using Common.Repository.Repository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OSP.Common.Repository.Repository.MainRepository
{
    public class RefCompanyRepository : IRefCompanyRepository<RefCompany>
    {
        #region Private Member Variables

        private OSPContext _context;
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
        public RefCompanyRepository(OSPContext context)
        {
            _context = context; 
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            _AbstractRepository = new AbstractRepository<RefCompany>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods
        public async Task<qryCompanyDetails> GetCompanyDetails(string CompanyType, string DeptCode)
        {
            try
            {
                qryCompanyDetails _qryCompanyDetails = await _context.qryCompanyDetails.FromSqlRaw("select A.CompanyCode,B.CompanyType,B.CompanyDesc,A.DeptCode,A.DeptDesc,A.DivisionCode,A.TerritoryCode from RefDepartment a inner join RefCompany b on a.CompanyCode = b.CompanyCode where b.CompanyType = '" + CompanyType +"' and a.DeptCode = '" + DeptCode +"' ").FirstAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList(); 
                return _qryCompanyDetails;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<IList<RefCompany>> GetCompanylist(string company)
        {
            try
            {
                IList<RefCompany> vlist = await _context.RefCompany.FromSqlRaw("select * from RefCompany where active = 1 and companydesc like '%" + company + "%'").ToListAsync();
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
                IList<RefCompany> vlist = await _context.RefCompany.FromSqlRaw("select * from RefCompany where active=1").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<RefCompany>> companies()
        {
            try
            {
                List<RefCompany> vlist = await _context.RefCompany.ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
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
                var companycode = await Task.FromResult(_context.RefCompany.Where(p => p.CompanyDesc.Contains(companydesc))
                                                                   .Select(p => p.CompanyCode).FirstOrDefault());

                return companycode;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetCompanyDescByCode(string companycode)
        {
            try
            {
                //string vlist = await _context.RefCompany.FromSqlRaw("select companycode from RefCompany where active=1 and companydesc like '"+ company + "'").FirstOrDefaultAsync();
                var companydesc = await Task.FromResult(_context.RefCompany.Where(p => p.CompanyCode.Contains(companycode))
                                                                   .Select(p => p.CompanyDesc).FirstOrDefault());

                return companydesc;
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
                IList<qryCompanyType> vlist = await _context.qryCompanyType.FromSqlRaw("select distinct a.companyType from RefCompany a inner join tblpersonaccess b on a.companytype=b.companytype inner join RefDepartment c on c.deptcode=b.DeptCode where a.active = 1 and b.personid ='" + personid + "'").ToListAsync();


                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetCompanyType(string CompanyCode)
        {
            try
            {
                return await _context.RefCompany.FromSqlRaw("select isnull(CompanyType,'') as [CompanyType] from RefCompany where CompanyCode='" + CompanyCode + "'").Select(a => a.CompanyType).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<RefDepartments> CompanyCode(string DeptDesc, string CompanyType)
        { 
            try
            {
                RefDepartments vlist = await _context.RefDepartment.FromSqlRaw("SELECT a.* FROM OSP.dbo.RefDepartment a inner join RefCompany b on a.CompanyCode = b.CompanyCode where a.DeptDesc = '" + DeptDesc + "' and b.CompanyType ='" + CompanyType + "'").FirstAsync();
                 
                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Public Functions







        #endregion
    }
}
