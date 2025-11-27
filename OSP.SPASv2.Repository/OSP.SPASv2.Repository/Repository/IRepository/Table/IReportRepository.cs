namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IReportRepository <TEntity> where TEntity : class
    {

        public Task<IList<RefReportType>> GetReportType(string PersonId);
        public Task<IList<RefReportname>> GetReportName(string PersonId);
    }
}
