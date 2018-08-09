using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestApplication.Models
{
  public class User
  {
    public int ID { get; set; }
    public string UserName { get; set; }
    public int GroupID { get; set; }
  }
}