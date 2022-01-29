using T034.Core.Dto.Common;

namespace T034.Core.Dto
{
    /// <summary>
    /// Новостная лента
    /// </summary>
    public class NewslineDto : AbstractDto<int>
    {
        public string Name { get; set; }
    }
}