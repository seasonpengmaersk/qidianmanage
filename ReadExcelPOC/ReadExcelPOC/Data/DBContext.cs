using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadExcelPOC.Models;

namespace ReadExcelPOC.Models
{
    public class DBContext : DbContext
    {
        public DBContext (DbContextOptions<DBContext> options)
            : base(options)
        {
        }
        

        public DbSet<ReadExcelPOC.Models.Terminal> Terminal { get; set; }
        public DbSet<ReadExcelPOC.Models.City> City { get; set; }
    }
}
