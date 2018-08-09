using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TestApplication.Models;

namespace TestApplication.DAL
{
  public class TeamContext : DbContext
  {
    public TeamContext() : base("TeamContext")
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }
  }
}