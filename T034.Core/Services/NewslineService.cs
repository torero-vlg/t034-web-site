using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using T034.Core.DataAccess;
using T034.Core.Dto;
using T034.Core.Entity;
using T034.Core.Services.Common;

namespace T034.Core.Services
{
    public interface INewslineService : IService
    {
        IEnumerable<NewslineDto> Select();

        NewslineDto Create(NewslineDto dto);
        NewslineDto Update(NewslineDto dto);
        NewslineDto Get(object id);
        OperationResult Delete(object id);

        /// <summary>
        /// Получить новости новостной ленты
        /// </summary>
        /// <param name="id">Код новостной ленты</param>
        /// <returns></returns>
        IEnumerable<NewsDto> GetNews(int id);
    }

    public class NewslineService : AbstractRepository<Newsline, NewslineDto, int>, INewslineService
    {
        public NewslineService(IBaseDb db, IMapper mapper)
            : base(db, mapper)
        { }

        public IEnumerable<NewsDto> GetNews(int id)
        {
            var news = Db.Where<News>(n => n.Newsline.Id == id).OrderByDescending(n => n.LogDate);
            return Mapper.Map<IEnumerable<NewsDto>>(news);
        }
    }
}
