using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.Common.Domain.View;
using DocumentFormat.OpenXml.Bibliography;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class ReportRepository : IReportRepository<RefReportType>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefReportType> _AbstractRepository;
        StringBuilder sb;
        //VendorbankaccountRules vrules = new VendorbankaccountRules();

        #endregion
        public ReportRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefReportType>(_context);
        }

        public async Task<IList<RefReportname>> GetReportName(string PersonId)
        {
            try
            {
                IList<RefReportname> vlist = await _context.RefReportname.FromSqlRaw("select * from RefReportname ").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<RefReportType>> GetReportType(string PersonId)
        {
            try
            {
                IList<RefReportType> vlist = await _context.RefReportType.FromSqlRaw("select * from RefReportType ").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
