using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Social_Models;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
    public int? ActivityId { get; set; }
    public Activity? Activity { get; set; }
}