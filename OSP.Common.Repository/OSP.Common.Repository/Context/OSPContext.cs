using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain;
using OSP.Common.Domain.References;
using OSP.Common.Domain.Tables;


namespace OSP.Common.Repository.Context
{
    public class OSPContext : DbContext
    {
        //public SPASv2Context()
        //{

        //}

        public OSPContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblUser>().HasKey(t => new { t.UserName });
            modelBuilder.Entity<RefCompany>().HasKey(t => new { t.CompanyCode });
            modelBuilder.Entity<RefChapel>().HasKey(t => new { t.ChapelCode });
            modelBuilder.Entity<RefBranch>().HasKey(t => new { t.BranchCode });

            modelBuilder.Entity<qryBranch>().HasKey(t => new { t.Branchcode });
            modelBuilder.Entity<qryCompanyType>().HasKey(t => new { t.CompanyType });
            modelBuilder.Entity<qryCompanyDetails>().HasNoKey();
            modelBuilder.Entity<TblSystemUpdateDtl>().HasKey(t => new { t.UpdateCode,t.DepartmentCode });
            modelBuilder.Entity<RefDepartments>().HasKey(t=> new {t.DeptCode,t.CompanyCode});

            modelBuilder.Entity<TblNotification>().HasKey(t => new { t.Idx });
            modelBuilder.Entity<TblEmployee>().HasKey(t => new { t.PersonID});
            modelBuilder.Entity<qryEmployee>().HasNoKey();
            modelBuilder.Entity<RefChapelEmail>().HasNoKey();

            modelBuilder.Entity<TblOutbox>().HasKey(t => new { t.MsgID });
            modelBuilder.Entity<TblInbox>().HasKey(t => new { t.MsgID });
            modelBuilder.Entity<TblRecipient>().HasKey(t => new { t.Autonumber });
            modelBuilder.Entity<TblSendemaildtl>().HasKey(t => new {
                t.EmailName,
                t.ReferenceNo
            });


    }
        public DbSet<TblSendemaildtl> TblSendEmailDtl { get; set; }
        public DbSet<TblOutbox> TblOutbox { get; set; }
        public DbSet<TblInbox> TblInbox { get; set; }
        public DbSet<TblUser> TblUser { get; set; }
        public DbSet<RefCompany> RefCompany { get; set; }
        public DbSet<RefChapel> RefChapel { get; set; }
        public DbSet<RefBranch> RefBranch { get; set; }
        public DbSet<RefDepartments> RefDepartment { get; set; }
        public DbSet<TblRecipient> TblRecipient { get; set; }
        public DbSet<qryBranch> qryBranch { get; set; }
        public DbSet<TblSystemUpdateDtl> TblSystemUpdateDtl { get; set; }
        public DbSet<qryCompanyType> qryCompanyType { get; set; }
        public DbSet<TblNotification> TblNotification { get; set; }
        public DbSet<qryCompanyDetails> qryCompanyDetails { get; set; }
        public DbSet<RefChapelEmail> RefChapelEmail { get; set; }

        public DbSet<TblEmployee> TblEmployee { get; set; }
        public DbSet<qryEmployee> qryEmployee { get; set; }
        internal IEnumerable<object> GetValidationErrors()
        {
            throw new NotImplementedException();
        }
    }
}
