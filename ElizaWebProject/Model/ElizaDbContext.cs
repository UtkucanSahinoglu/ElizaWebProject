using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElizaWebProject.Model
{
    public class ElizaDbContext : IdentityDbContext
    {
        public ElizaDbContext(DbContextOptions<ElizaDbContext> options) : base(options)
        {

        }
    }

}
