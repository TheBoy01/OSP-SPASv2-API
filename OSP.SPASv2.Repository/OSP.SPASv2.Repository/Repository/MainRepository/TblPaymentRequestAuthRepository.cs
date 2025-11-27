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
    public class TblPaymentRequestAuthRepository : ITblPaymentRequestAuthRepository<TblPaymentRequestAuth>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblPaymentRequestAuth> _AbstractRepository;
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
        public TblPaymentRequestAuthRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblPaymentRequestAuth>(_context);
        }

        #endregion

        public async Task<int> GetDeniedCount(string ReqNo)
        {
			try
			{
                return await Task.FromResult(_context.TblPaymentRequestAuth.Where(a => a.Reqno.Equals(ReqNo) && a.StatusType.ToUpper().Equals("DN")).ToList().Count());
			}
			catch (Exception ex)
			{
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> UpdatePayment(string ReqNo, string Remarks, string StatusType, DateTime AuthDate, int AuthLevel, string UserCode)
        {
            
            try
            {
                await Task.FromResult(_context.Database.ExecuteSqlRaw("exec sp_UpdatePaymentStatus '" + ReqNo + "', '" + Remarks +"','" + StatusType +"','" + AuthDate +"','" + AuthLevel +"', '" + UserCode +"'"));
                _response = new TblResponse
                {
                    Status = "SUCCESS",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "SUCCESS",
                    MethodName = "SP UPDATE PAYMENT STATUS",
                    TrxNo = ReqNo,
                    UniqueInfo = "1"
                };

                return await Task.FromResult(_response);
            }
            catch (Exception e)
            {
                _response = new TblResponse
                {
                    Status = "FAILED",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "ERROR: " + e.Message,
                    MethodName = "SP UPDATE PAYMENT STATUS",
                    TrxNo = ReqNo,
                    UniqueInfo = "1"
                };

                return _response;
            }
        }
    }
}
