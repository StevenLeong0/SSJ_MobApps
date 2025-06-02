using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole
{
    public override required string? Name{ get; set; }
}