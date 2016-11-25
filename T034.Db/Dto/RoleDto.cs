using Db.Dto.Common;

namespace Db.Dto
{
    public class RoleDto : AbstractDto<int>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool Selected { get; set; }
    }
}
