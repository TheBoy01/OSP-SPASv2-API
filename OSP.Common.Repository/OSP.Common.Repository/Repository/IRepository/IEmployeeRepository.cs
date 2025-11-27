using OSP.Common.Domain.References;

namespace OSP.Common.Repository.IRepository.Table
{
    public interface IEmployeeRepository<TEntity> where TEntity : class
    {
        public Task<TblEmployee> ReadTblEmployee(string personid);
        public Task<qryEmployee> GetEmployeeDetails(string personid);
    }
}
