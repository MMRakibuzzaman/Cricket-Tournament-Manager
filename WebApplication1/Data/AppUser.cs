using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Data;

public class AppUser:IdentityUser
{
    public string? Name { get; set; }
    public string? CellPhone { get; set; }
    public string? Country { get; set; }
}