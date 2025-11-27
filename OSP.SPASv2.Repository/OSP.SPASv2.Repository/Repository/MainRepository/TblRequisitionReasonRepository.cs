using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.Common.Domain.View;
//using System.Data.Entity;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblRequisitionReasonRepository : ITblRequisitionReasonRepositioryy<TblRequisitionReason>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblRequisitionReason> _AbstractRepository;
        //GenericRepository<TblPurchaseorderhdr> _GenericRepository;
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
        public TblRequisitionReasonRepository(SPASv2Context context)
        {
            _context = context;

            _AbstractRepository = new AbstractRepository<TblRequisitionReason>(_context);
            //_GenericRepository = new GenericRepository<TblPurchaseorderhdr>(_context);
        }

        public async Task<TblResponse> Create(TblRequisitionReason entity)
        {
            try
            {
                await _AbstractRepository.Insert(entity);
                return await Task.FromResult(_response);
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
