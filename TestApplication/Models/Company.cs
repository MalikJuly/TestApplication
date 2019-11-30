using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestApplication.Models
{
  public class Company
  {
    [Key]
    public int company_id { get; set; }
    public string name { get; set; }
    public string website { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public int zip { get; set; }
    public string phone { get; set; }

    public ICollection<Contact> contacts { get; set; }
  }
}