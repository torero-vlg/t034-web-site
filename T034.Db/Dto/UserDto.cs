using System.Collections.Generic;
using Db.Dto.Common;

namespace Db.Dto
{
    public class UserDto : AbstractDto<int>
    {
        public UserDto()
        {
            UserRoles = new List<RoleDto>();
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<RoleDto> UserRoles { get; set; }
    }
}
