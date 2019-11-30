using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestApplication.DAL;
using TestApplication.Models;

namespace TestApplication.Controllers
{
  public class TeamController : Controller
    {
        private TeamContext db = new TeamContext();

        public ActionResult Index()
        {
            ViewBag.GroupList = GroupList();
            return View();
        }

        [HttpPost]
        public string GroupDetails(string searchString)
        {
            var group = !String.IsNullOrEmpty(searchString) ? db.Groups.FirstOrDefault(x => x.GroupName == searchString) : null;
            var firstChildGroups = group != null ? db.Groups.Where(x => x.ParentGroupID == group.ID).ToList() : null;
            var allChildGroups = new List<Group>();
            allChildGroups.AddRange(firstChildGroups);
            foreach (var child in firstChildGroups)
            {
              var childGroups = db.Groups.Where(x => x.ParentGroupID == child.ID).ToList();
              allChildGroups.AddRange(childGroups);
            }

            var users = db.Users.Where(x => x.GroupID == group.ID).ToList();
            foreach (var item in allChildGroups)
            {
              var groupUsers = db.Users.Where(x => x.GroupID == item.ID).ToList();
              users.AddRange(groupUsers);
            }
            var jsonSerialiser = new JavaScriptSerializer();
            var result = jsonSerialiser.Serialize(users.Select(m => new { m.UserName }).Distinct());
          
            return result;
        }

        public List<SelectListItem> GroupList()
        {
            List<SelectListItem> groupList = new List<SelectListItem>
            {
                new SelectListItem() { Value = "0", Text = "" }
            };
            var groups = db.Groups.ToList();
            foreach (var item in groups)
                groupList.Add(new SelectListItem() { Value = item.ID.ToString(), Text = item.GroupName });
            
            return groupList;
        }
    }
}
