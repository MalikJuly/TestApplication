using System;

namespace TestApplication.Models
{
  public class ContactViewModel
  {
    public int vid { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string lifecyclestage { get; set; }
    public string addedAt { get; set; }
    public string lastmodifieddate { get; set; }
    public int associated_company_id { get; set; }
    public string companyname { get; set; }
    public string companywebsite { get; set; }
    public string companycity { get; set; }
    public string companystate { get; set; }
    public int companyzip { get; set; }
    public string companyphone { get; set; }
  }
}