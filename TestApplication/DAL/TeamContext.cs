using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TestApplication.Models;

namespace TestApplication.DAL
{
  public class TeamContext : DbContext
  {
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Company> Companies { get; set; }

    static TeamContext()// : base("TeamContext")
    {
      Database.SetInitializer<TeamContext>(new TeamInitializer());
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }
  }
}