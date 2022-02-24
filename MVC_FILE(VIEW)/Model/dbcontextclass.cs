using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVC_FILE_VIEW_.ViewModel;
using System.Linq;

namespace MVC_FILE_VIEW_.Model
{
    //(:dbcontext)but identityDbContext inherit from DbContext so it is the same 
    //<> we add that because we create class called ApplicationUser inhert from IdentityUser and to add new property from ApplicationUser to database  
    public class dbcontextclass:IdentityDbContext<ApplicationUser> 
    {
        public dbcontextclass(DbContextOptions<dbcontextclass> options):base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.seed();
            //tell us if u want delete any role u must know it is not in user and if u want to delete it u must delete user first
            foreach(var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e=>e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
