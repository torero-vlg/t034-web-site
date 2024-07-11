using T034.Core.Dto.Common;

namespace T034.Core.Dto
{
    public class MenuItemDto : AbstractDto<int>
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public string Active { get; set; }

        public int OrderIndex { get; set; }

        public int? ParentId { get; set; }

        public string Parent { get; set; }
    }
}