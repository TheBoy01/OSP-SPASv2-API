

using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class tmpPaymentRequestInventoryRepository : ItmpPaymentRequestInventoryRepository<tmpPaymentRequestInventory>
    {


        private SPASv2Context _context;
        AbstractRepository<tmpPaymentRequestInventory> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        public tmpPaymentRequestInventoryRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<tmpPaymentRequestInventory>(_context);
        }

        public async Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventory()
        {
            try
            {
                IList<tmpPaymentRequestInventory> vlist = await _context.tmpPaymentRequestInventory.FromSqlRaw("select * from tmpPaymentRequestInventory ").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventoryA(string audituser)
        {
            try
            {
                IList<tmpPaymentRequestInventory> vlist = await _context.tmpPaymentRequestInventory.FromSqlRaw("select * from tmpPaymentRequestInventory where audituser='"+ audituser +"'").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();


                
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventoryA(string audituser,string prno)
        {
            try
            {
                IList<tmpPaymentRequestInventory> vlist = await _context.tmpPaymentRequestInventory.FromSqlRaw("select * from tmpPaymentRequestInventory where audituser='" + audituser + "' and prno='"+ prno +"'").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();



                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<TblResponse> PosttmpPaymentRequestInventory(tmpPaymentRequestInventory tmp)
        {
            await _AbstractRepository.Insert(tmp);
            return await Task.FromResult(_response);
        }
    }
}
