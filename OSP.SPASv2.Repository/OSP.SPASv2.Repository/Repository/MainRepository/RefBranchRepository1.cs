
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
using OSP.SPASv2.Domain.View;
using Microsoft.Data.SqlClient;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository
{
    public class RefBranchRepository1 : IRefBranchRepository<RefBranch>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefBranch> _AbstractRepository;
        StringBuilder sb;
        //VendorRules vrules = new VendorRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public RefBranchRepository1(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefBranch>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion

        #region Public Functions

        public async Task<IList<RefBranch>> GetBranchlist()
        {

            try
            {
                IList<RefBranch> vlist = await _context.RefBranch.FromSqlRaw("select * from Refbranch").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<RefBranch>> GetBranches(string branchdesc, string company)
        {

            try
            {
                //IList<RefBranch> vlist = await _context.RefBranch.FromSqlRaw("select * from Refbranch where companycode = '"+ company +"' and branchdesc like '%"+ branchdesc +"%'").ToListAsync();
                if (string.IsNullOrEmpty(branchdesc))
                {
                    IList<RefBranch> vlist = await _context.RefBranch.FromSqlRaw("select a.* from refbranch a \r\ninner join refcompany b on a.CompanyCode=b.companycode and b.companytype='LIFEPLAN' and a.active=1\r\ninner join tblpersonaccess c on b.CompanyType=c.CompanyType and a.branchcode=c.deptcode ").ToListAsync();
                    return vlist;
                }
                else
                {
                    IList<RefBranch> vlist = await _context.RefBranch.FromSqlRaw("select * from refbranch where companycode in (select companycode from refcompany where companytype='lifeplan') and  branchdesc like '%" + branchdesc + "%'").ToListAsync();
                    return vlist;
                }



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public async Task<qryBranch> GetBranchdetails(string companycode, string branchcode)
        {

            try
            {


                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                var vlist = await _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails @p0, @p1", companycode, branchcode).ToListAsync();

                return vlist.FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }



        #endregion

    }
}

