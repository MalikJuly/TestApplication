using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApplication.Models
{
  public class Contact
  {
    [Key]
    public int vid { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string lifecyclestage { get; set; }
    public DateTime addedAt { get; set; }
    public DateTime? lastmodifieddate { get; set; }
    [ForeignKey("Company")]
    public int associated_company_id { get; set; }
    public Company Company { get; set; }
  }
}