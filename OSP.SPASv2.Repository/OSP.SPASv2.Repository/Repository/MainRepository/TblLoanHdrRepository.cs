using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblLoanHdrRepository : ITblLoanHdrRepository<TblLoanhdr>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblLoanhdr> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public TblLoanHdrRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblLoanhdr>(_context);
        }
        #endregion

        public async Task<TblResponse> CreateLoanHdr(TblLoanhdr entity)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                _context.Database.ExecuteSqlRaw("exec sp_CreateLoanHdr '" + entity.LAFNo + "'," + entity.AppliedLoan + "");
                return await Task.FromResult(_response);
                 
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

      


    }
}
