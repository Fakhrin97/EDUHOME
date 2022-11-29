using Microsoft.AspNetCore.Identity;

namespace EDUHOME.DAL.Entities
{
    public class User : IdentityUser
    {
        public string? FristName {  get; set; } 
        public string? LastName {  get; set; } 
        
    }
}
