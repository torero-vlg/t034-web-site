using T034.Core.Dto.Common;

namespace T034.Core.Dto
{
    public class RoleDto : AbstractDto<int>
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public bool Selected { get; set; }
    }
}
