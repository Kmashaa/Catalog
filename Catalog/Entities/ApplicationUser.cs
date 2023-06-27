using Microsoft.AspNetCore.Identity;

namespace Catalog.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public int? RoleId { get; set; }

    }
}
