using System.Collections.Generic;

namespace Db.Dto
{
    public class UserDto
    {
        public UserDto()
        {
            UserRoles = new List<RoleDto>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<RoleDto> UserRoles { get; set; }
    }
}
