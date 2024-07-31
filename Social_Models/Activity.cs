namespace Social_Models;

public class Activity : BaseEntity
{
    public string Title { get; set; }
    public string CreatedBy { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public DateTime Date { get; set; }
    public List<ApplicationUser>? ApplicationUsers { get; set; }
}