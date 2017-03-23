using Db.Dto.Common;

namespace Db.Dto
{
    /// <summary>
    /// Новостная лента
    /// </summary>
    public class NewslineDto : AbstractDto<int>
    {
        public string Name { get; set; }
    }
}