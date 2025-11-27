using OSP.Common.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Repository.IRepository;

using System.Text;
using OSP.Common.Domain.Tables;
using System.Reflection;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.Repository.IRepository;
using Common.Repository.Repository;
using System.Data;


namespace OSP.SPASv2.Repository.Repository
{
    public class RefDepartmentRepository : IRefDepartmentRepository<RefDepartments>
    {

        #region Private Member Variables

        private OSPContext _context;
        private readonly IGenericRepository<RefDepartments> _repository;

        AbstractRepository<RefDepartments> _AbstractRepository;
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
        public RefDepartmentRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            _AbstractRepository = new AbstractRepository<RefDepartments>(_context);
            _repository = new GenericRepository<RefDepartments>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        //public async Task<TblResponse> Create(RefDepartment entity)
        //{
        //    await _AbstractRepository.Insert(entity);
        //    return await Task.FromResult(_response);
        //}

        //public async Task<TblResponse> Update(RefDepartment entity)
        //{
        //    var oldEntity = this.Read(entity.VendorCode);
        //    _AbstractRepository.Update(oldEntity, entity);
        //    new Task(() => { TrailEdit(oldEntity, entity, "RefDepartment", "Department", entity.VendorCode); }).Start();
        //    return await Task.FromResult(_response);
        //}

        //public async Task<TblResponse> Delete(RefDepartment entity)
        //{
        //     _AbstractRepository.Delete(entity);
        //    return await Task.FromResult(_response);
        //}

        //public async Task<TblResponse> Delete(object Primarykey, object Primarykey2)
        //{
        //     _AbstractRepository.DeleteByComposite(Primarykey, Primarykey2);
        //    return await Task.FromResult(_response);
        //}

        #endregion
        #region Public Functions

        public async Task<IList<RefDepartments>> GetAllObjects()
        {
            try
            {
                var vlist = await Task.FromResult(_context.RefDepartment.FromSqlRaw("select * from RefDepartment").ToList());
                return vlist;
            }
           catch (Exception es)
           {
                throw new Exception(es.Message);
            }
        }

        public async Task<IList<RefDepartments>> GetAllbyCompanycode(string companycode)
        {

            try
            {
                var entities = await _repository.GetAllIListAsync(filter: entity => entity.DeptCode == companycode,
        orderBy: query => query.OrderBy(entity => entity.DeptDesc));
                return entities;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<RefDepartments>> GetDeptByPersonID(string personid,string companytype)
        {
            try
            {
                IList<RefDepartments> vlist = await _context.RefDepartment.FromSqlRaw("select a.* from refdepartment a inner join refcompany b on a.CompanyCode=b.companycode inner join tblpersonaccess c on b.CompanyType = c.CompanyType and a.deptcode = c.deptcode where c.personid = '"+ personid +"' and c.companytype='"+ companytype +"' ").ToListAsync();
                return vlist;



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<IList<qryCompanyDetails>> GetAllCompanyDetails()
        {
            try
            {
                IList<qryCompanyDetails> vlist = await _context.qryCompanyDetails.ToListAsync();
                return vlist;



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<RefChapelEmail> GetEmailPerChapel(string chapelcode)
        {
            try
            {
                RefChapelEmail vlist = await _context.RefChapelEmail.FromSqlRaw("select * from RefChapelEmail where chapelcode = '"+ chapelcode + "'  ").FirstOrDefaultAsync();
                return vlist;



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        #endregion

    }
}

