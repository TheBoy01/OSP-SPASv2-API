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
using OSP.SPASv2.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository
{
    public class ResponseRepository : IResponseRepository<TblResponse>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblResponse> _AbstractRepository;
        StringBuilder sb;

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        
        public ResponseRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblResponse>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public async Task CreateResponse(TblResponse entity)
        {
            await _AbstractRepository.Insert(entity);
        }


        public async Task<TblResponse> CreateResponse(string UniquInfo ,string status,string errormsg,string MethodN)
        {
            TblResponse _r = new TblResponse();
            {
                _r.TrxNo = DateTime.Now.ToString("MMMYYYYhhmmss.fff tt"); 
                _r.UniqueInfo = UniquInfo;
                _r.Status = status;
                _r.ErrorMessage = errormsg;
                _r.MethodName = MethodN;
                _r.AuditDate = DateTime.Now;
            }

            //await _AbstractRepository.Insert(_r);
            return await Task.FromResult(_r);

        }

        public async Task<TblResponse> CreateResponse1(string UniquInfo, string errormsg, string MethodN)
        {
            TblResponse _r = new TblResponse();
            {
                _r.TrxNo = DateTime.Now.ToString("MMMyyyyhhmmss");
                _r.UniqueInfo = UniquInfo;
                _r.MethodName = MethodN;
                _r.AuditDate = DateTime.Now;
            }
            if (string.IsNullOrEmpty(errormsg))
            {
                _r.ErrorMessage = "SUCCESFULLY SAVE.";
                _r.Status = "SUCCESS";
            }
            else
            {
                _r.ErrorMessage = errormsg;
                _r.Status = "FAILED";
            }

            await _AbstractRepository.Insert(_r);





            return await Task.FromResult(_r);

        }

        #endregion

        #region Public Functions


        #endregion

    }
}


