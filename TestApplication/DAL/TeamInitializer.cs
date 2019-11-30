using System;
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

      var companies = new List<Company>
            {
            new Company{company_id = 1, name = "Company1", website = "http://Company1.com", city = "Company1City", state = "Company1State", zip = 61001, phone = "1234567890"},
            new Company{company_id = 2, name = "Company2", website = "http://Company2.com", city = "Company2City", state = "Company2State", zip = 61002, phone = "1234567891"},
            new Company{company_id = 3, name = "Company3", website = "http://Company3.com", city = "Company3City", state = "Company3State", zip = 61003, phone = "1234567892"},
            new Company{company_id = 4, name = "Company4", website = "http://Company4.com", city = "Company4City", state = "Company4State", zip = 61004, phone = "1234567893"},
            new Company{company_id = 5, name = "Company5", website = "http://Company5.com", city = "Company5City", state = "Company5State", zip = 61005, phone = "1234567894"}
            };
      companies.ForEach(s => context.Companies.Add(s));
      context.SaveChanges();

      var contacts = new List<Contact>
            {
            new Contact{vid = 1, firstname = "Cont1FN", lastname = "Cont1LN", lifecyclestage = "Added",   associated_company_id = 1, addedAt = new DateTime(2019, 10, 25), lastmodifieddate = null},
            new Contact{vid = 2, firstname = "Cont2FN", lastname = "Cont2LN", lifecyclestage = "Added",   associated_company_id = 2, addedAt = new DateTime(2019, 11, 25), lastmodifieddate = null},
            new Contact{vid = 3, firstname = "Cont3FN", lastname = "Cont3LN", lifecyclestage = "Updated", associated_company_id = 3, addedAt = new DateTime(2019, 10, 27), lastmodifieddate = new DateTime(2019, 10, 28)},
            new Contact{vid = 4, firstname = "Cont4FN", lastname = "Cont4LN", lifecyclestage = "Updated", associated_company_id = 4, addedAt = new DateTime(2019, 10, 28), lastmodifieddate = new DateTime(2019, 11, 02)},
            new Contact{vid = 5, firstname = "Cont5FN", lastname = "Cont5LN", lifecyclestage = "Updated", associated_company_id = 5, addedAt = new DateTime(2019, 10, 29), lastmodifieddate = new DateTime(2019, 11, 29)}
            };
      contacts.ForEach(s => context.Contacts.Add(s));
      context.SaveChanges();
    }
    }
}