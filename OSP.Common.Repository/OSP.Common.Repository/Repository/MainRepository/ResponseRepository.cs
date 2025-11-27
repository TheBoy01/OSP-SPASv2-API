
using Microsoft.EntityFrameworkCore;
using OSP.Common.Repository.IRepository;
using OSP.Common.Repository.Rules;

using System.Text;
using OSP.Common.Domain.Tables;

using Microsoft.AspNetCore.Mvc;
using OSP.Common.Domain;
using OSP.Common.Repository.IRepository.Table;
using OSP.Common.Repository.Repository;
using OSP.Common.Repository.Context;
using Common.Repository.Repository;


namespace OSP.Common.Repository.Repository
{
    public class ResponseRepository : IResponseRepository<TblResponse>
    {

        #region Private Member Variables

        private OSPContext _context;
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
        
        public ResponseRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
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
                _r.TrxNo = string.Empty;
                _r.UniqueInfo = UniquInfo;
                _r.Status = status;
                _r.ErrorMessage = errormsg;
                _r.MethodName = MethodN;
                _r.AuditDate = DateTime.Now;
            }
            await _AbstractRepository.Insert(_r);
            return await Task.FromResult(_r);

        }

        #endregion

        #region Public Functions


        #endregion

    }
}


