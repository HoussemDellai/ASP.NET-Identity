using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNetIdentity.WebApi.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            //Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<ApplicationDbContext>());
            //Database.CreateIfNotExists();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}