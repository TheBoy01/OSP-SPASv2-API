using OSP.Common.Domain.References;

namespace OSP.Common.Repository.Repository.IRepository
{
    public interface IRefDepartmentRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefDepartments>> GetAllObjects();
        public Task<IList<RefDepartments>> GetAllbyCompanycode(string companycode);
        public Task<IList<RefDepartments>> GetDeptByPersonID(string personid,string companytype);
    }
}
