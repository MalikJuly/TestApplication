using System.Collections.Generic;
using TestApplication.Models;

namespace TestApplication.DAL
{
  public class TeamInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TeamContext>
  {
    protected override void Seed(TeamContext context)
    {
      var groups = new List<Group>
            {
            new Group{ID = 1, GroupName = "Group1", ParentGroupID = 0},
            new Group{ID = 2, GroupName = "Group2", ParentGroupID = 0},
            new Group{ID = 3, GroupName = "Group3", ParentGroupID = 2},
            new Group{ID = 4, GroupName = "Group4", ParentGroupID = 3},
            new Group{ID = 5, GroupName = "Group5", ParentGroupID = 2}
            };

      groups.ForEach(s => context.Groups.Add(s));
      context.SaveChanges();

      var users = new List<User>
            {
            new User{ID = 1, UserName = "User1", GroupID = 1},
            new User{ID = 2, UserName = "User2", GroupID = 2},
            new User{ID = 3, UserName = "User3", GroupID = 3},
            new User{ID = 4, UserName = "User4", GroupID = 4},
            new User{ID = 5, UserName = "User5", GroupID = 1},
            new User{ID = 6, UserName = "User1", GroupID = 3},
            new User{ID = 7, UserName = "User2", GroupID = 4}
            };
      users.ForEach(s => context.Users.Add(s));
      context.SaveChanges();
    }
    }
}