using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using OSP.SPASv2.Repository.Rules;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.MainRepository;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Domain;
using OSP.Common.Domain.References;
using OSP.SPASv2.Repository.IRepository.Table;
using OSP.SPASv2.Domain.References;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefRegionRepository : IRefRegionRepository<RefRegion>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefRegion> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion


        #region Contructor
        public RefRegionRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefRegion>(_context);
        }
        #endregion


        public async Task<IList<RefRegion>> GetRegionList()
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<RefRegion> vlist = await _context.RefRegion.FromSqlRaw("select * from RefRegion where Active=1 order by RegionCode Asc").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<qryChapelBranchDetails> GetChapelDetails(string Chapelcode)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                qryChapelBranchDetails vlist = await _context.qryChapelBranchDetails.FromSqlRaw("select ChapelCode,GCMID,GCMName,Email from tblGCM_April_2024 where Chapelcode = '"+ Chapelcode + "'").FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<qryChapelBranchDetails> GetChapelDetailsbygcmname(string gcmcode)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                qryChapelBranchDetails vlist = await _context.qryChapelBranchDetails.FromSqlRaw("select top 1 ChapelCode,GCMID,GCMName,Email from tblGCM_April_2024 where gcmid = '" + gcmcode + "'").FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
