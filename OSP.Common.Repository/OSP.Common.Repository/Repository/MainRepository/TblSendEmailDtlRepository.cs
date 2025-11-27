using OSP.Common.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Repository.IRepository;
using OSP.Common.Domain;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.Common.Repository.Context;
using Common.Repository.Repository;
using OSP.Common.Repository.Repository.IRepository;
using System.Security.Principal;

namespace OSP.SPASv2.Repository.Repository
{
    public class SendemaildtlRepository : ISendEmailDtlRepository<TblSendemaildtl>
    {

        #region Private Member Variables

        private OSPContext _context;
        AbstractRepository<TblSendemaildtl> _AbstractRepository;
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
        public SendemaildtlRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            _AbstractRepository = new AbstractRepository<TblSendemaildtl>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public async Task<TblResponse> Create(TblSendemaildtl entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Update(TblSendemaildtl entity)
        {
            //var oldEntity = this.Read(entity.VendorCode);
          //  await _AbstractRepository.Update(oldEntity, entity);
            //new Task(() => { TrailEdit(oldEntity, entity, "TblSendemaildtl", "Sendemaildtl", entity.VendorCode); }).Start();
            return await Task.FromResult(_response);
        }

        //public async Task<TblResponse> Delete(TblSendemaildtl entity)
        //{
        //    await _AbstractRepository.Delete(entity);
        //    return await Task.FromResult(_response);
        //}

        //public async Task<TblResponse> Delete(object Primarykey, object Primarykey2)
        //{
        //    await _AbstractRepository.DeleteByComposite(Primarykey, Primarykey2);
        //    return await Task.FromResult(_response);
        //}

        #endregion
        #region Public Functions

        public async Task<IList<TblSendemaildtl>> GetAllObjects()
        {
            try
            {
                var vlist = await Task.FromResult(_context.TblSendEmailDtl);
                return vlist.ToList();
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<IList<TblSendemaildtl>> GetAllPending()
        {
            try
            {
                var vlist = await Task.FromResult(_context.TblSendEmailDtl.Where(a=>a.StatusType == "PENDING").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<IList<TblSendemaildtl>> UpdateStatus(List<TblSendemaildtl> tblSendemaildtls)
        {
            try
            {
                tblSendemaildtls = new List<TblSendemaildtl>() ;
                TblSendemaildtl tblSendemaildtl = new TblSendemaildtl() { 
                ReferenceNo = ""};


                var dbRecords = await _context.TblSendEmailDtl.Where(a => a.StatusType == "PENDING").ToListAsync();

               
                foreach (var dbRecord in dbRecords)
                {
                    
                    var updateRecord = tblSendemaildtls.FirstOrDefault(t => t.ReferenceNo == dbRecord.ReferenceNo && t.EmailName == dbRecord.EmailName);

                    if (updateRecord != null)
                    {
                        // Update the StatusType of the database record
                        dbRecord.StatusType = updateRecord.StatusType;
  
                        _AbstractRepository.Update(dbRecord);
                    }
                }
               
               

                // Return the updated list of records
                return dbRecords;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        #endregion

    }
}

