using Azure;
using Common.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain;
using OSP.Common.Domain.Params;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.IRepository;
using OSP.Common.Repository.IRepository.Table;
using OSP.Common.Repository.Repository.IRepository;
using System.Data;
using System.Text;

namespace OSP.Common.Repository.Repository.MainRepository
{
    public class TblOutboxRepository : ITblOutboxRepository<TblOutbox>
    {
        private OSPContext _context;
        AbstractRepository<TblOutbox> _AbstractRepository;
        StringBuilder sb; 
        TblResponse _response = new TblResponse();

        public TblOutboxRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPIS2");
            _AbstractRepository = new AbstractRepository<TblOutbox>(_context);
            sb = new StringBuilder();
        }
        public async Task<TblResponse> SaveToOutbox(InfoTextParams InfoTextParams)
        {
            try
            {
                sb = new StringBuilder();
                DateTime DateTimeSent = DateTime.Now; 

                sb.Append("exec sp_savetooutbox '" + InfoTextParams.MobileNo.TrimStart().TrimEnd() + "','" + InfoTextParams.BodyMessage.Replace("'", "''") + "','" + DateTimeSent
                          + "','" + 1 + "','" + InfoTextParams.UserID + "','" + InfoTextParams.ComNum + "'");

                await Task.FromResult(_context.Database.ExecuteSqlRaw(sb.ToString()));
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                return _response;
            }
        }

        public async Task<TblOutbox> GetRecentMsgStatus(InfoTextParams InfoTextParams)
        {
            try
            {
                sb = new StringBuilder();
                DateTime DateTimeSent = DateTime.Now;
                var DbContext = DbContextFactory.Create("SPLPIS2");
                _AbstractRepository = new AbstractRepository<TblOutbox>(DbContext);

                sb.Append("Select top 1 * from Outbox where MPN='" + InfoTextParams.MobileNo.TrimStart().TrimEnd() + "' and COMNum='" + InfoTextParams.ComNum +"' order by Datestamp desc");

                return await DbContext.TblOutbox.FromSqlRaw(sb.ToString()).FirstOrDefaultAsync();
            
                //await Task.FromResult(DbContext.Database.ExecuteSqlRaw(sb.ToString()));
                //return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                return new TblOutbox();
            }
        }
    }
}
