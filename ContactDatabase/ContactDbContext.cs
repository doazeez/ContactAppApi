using ContactDatabase.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAndModel
{
    public class ContactDbContext:IdentityDbContext<User>
    {
        public ContactDbContext(DbContextOptions<ContactDbContext> options):base(options)
        {

        }
    }
}
