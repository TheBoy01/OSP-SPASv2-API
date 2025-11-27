using Microsoft.EntityFrameworkCore;

namespace OSP.SPASv2.Repository.Context
{
    public static class DbContextFactory
    {

        public static Dictionary<string, string> ConnectionStrings { get; set; }

        public static void SetConnectionString(Dictionary<string, string> connStrs)
        {
            ConnectionStrings = connStrs;
        }

        public static SPASv1Context Create(string connID)
        {
            if (!string.IsNullOrEmpty(connID))
            {
                var connStr = ConnectionStrings[connID];
                var optionsBuilder = new DbContextOptionsBuilder<SPASv1Context>();
                optionsBuilder.UseSqlServer(connStr);
                return new SPASv1Context(optionsBuilder.Options);
            }
            else
            {
                throw new ArgumentNullException("ConnectionId");
            }
        }

    }
}
