namespace TestApplication.Models
{
  public class Group
  {
    public int ID { get; set; }
    public string GroupName { get; set; }
    public int ParentGroupID { get; set; }
  }
}