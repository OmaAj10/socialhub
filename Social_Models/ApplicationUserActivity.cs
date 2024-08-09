namespace Social_Models;

public class ApplicationUserActivity : BaseEntity
{
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }

    public int ActivityId { get; set; }
    public Activity Activity { get; set; }
}