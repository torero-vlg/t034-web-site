using System;
using Db.Dto.Common;

namespace Db.Dto
{
    /// <summary>
    /// Новость
    /// </summary>
    public class NewsDto : AbstractDto<int>
    {
        public string Body { get; set; }

        public string Title { get; set; }

        public string Resume { get; set; }

        public DateTime LogDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int NewslineId { get; set; }
        public string Newsline { get; set; }
    }
}