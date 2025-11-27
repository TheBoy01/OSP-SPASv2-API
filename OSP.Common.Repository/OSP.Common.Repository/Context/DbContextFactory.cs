using Microsoft.EntityFrameworkCore;
using OSP.Common.Repository.Context;

namespace OSP.Common.Repository.Context
{
    public static class DbContextFactory
    {

        public static Dictionary<string, string> ConnectionStrings { get; set; }

        public static void SetConnectionString(Dictionary<string, string> connStrs)
        {
            ConnectionStrings = connStrs;
        }

        public static OSPContext Create(string connID)
        {
            if (!string.IsNullOrEmpty(connID))
            {
                var connStr = ConnectionStrings[connID];
                var optionsBuilder = new DbContextOptionsBuilder<OSPContext>();
                optionsBuilder.UseSqlServer(connStr);
                return new OSPContext(optionsBuilder.Options);
            }
            else
            {
                throw new ArgumentNullException("ConnectionId");
            }
        }

    }
}
