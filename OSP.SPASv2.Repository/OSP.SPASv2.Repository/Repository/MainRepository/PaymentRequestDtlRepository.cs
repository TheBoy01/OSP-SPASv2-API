using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblPaymentRequestDtlRepository : IPaymentRequestDtlRepository<TblPaymentrequestdtl>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblPaymentrequestdtl> _AbstractRepository;
        GenericService<TblPaymentrequestdtl> _Genericservice;
        TblResponse _response = new TblResponse();
        StringBuilder sb;
        //PaymentrequesthdrRules vrules = new PaymentrequesthdrRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public TblPaymentRequestDtlRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblPaymentrequestdtl>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        public async Task<TblResponse> CreatePaymentRequest(TblPaymentrequestdtl entity)
        {
            try
            {
                await _AbstractRepository.Insert(entity);
                //await _Genericservice.Insert(entity);
                return await Task.FromResult(_response);
            }
            catch (DbUpdateException ex)
            {

                throw new Exception(ex.Message);
            }

           
        }






        #endregion
        #region Public Functions

        //public async Task<IList<TblPaymentrequesthdr>> GetAllObjects()
        //{
        //    Try
        //    {
        //        var vlist = await Task.FromResult(_context.TblPaymentrequesthdr.FromSqlRaw("select * from TblPaymentrequesthdr").ToList());
        //        return vlist;
        //    }
        //   catch (Exception es)
        //    {
        //        throw new Exception(es.Message);
        //    }
        //}

        #endregion

    }
}
