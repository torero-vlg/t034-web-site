using System.Collections.Generic;

namespace T034.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<RoleViewModel> UserRoles { get; set; }
    }
}