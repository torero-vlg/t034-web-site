using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using T034.Core.DataAccess;
using T034.Core.Dto;
using T034.Core.Entity;
using T034.Core.Profiles;
using T034.Core.Services.Common;

namespace T034.Core.Services
{
    public interface IMenuItemService : IService
    {
        IEnumerable<MenuItemDto> Select();
        MenuItemDto ByUrl(string url);
        OperationResult Delete(int menuItemId);
    }

    public class MenuItemService : IMenuItemService
    {
        private readonly IBaseDb _db;

        public MenuItemService(IBaseDb db)
        {
            _db = db;
        }

        /// <summary>
        /// Маппер
        /// </summary>
        protected IMapper Mapper => AutoMapperConfig.Mapper;

        public IEnumerable<MenuItemDto> Select()
        {
            var list = new List<MenuItemDto>();
            var items = _db.Select<MenuItem>();
            list = Mapper.Map(items, list);
            return list;
        }

        public MenuItemDto ByUrl(string url)
        {
            var item = _db.Where<MenuItem>(i => i.Url == url).FirstOrDefault();
            var dto = Mapper.Map<MenuItemDto>(item);
            return dto;
        }

        public OperationResult Delete(int menuItemId)
        {
            if(_db.Where<MenuItem>(m => m.Parent.Id == menuItemId).Any())
                return new OperationResult { Status = StatusOperation.Error, Message = "Существует пункт меню, у которого удаляемый является родителем"};

            var item = _db.Get<MenuItem>(menuItemId);
            var result = _db.Delete(item);
            return new OperationResult { Status = StatusOperation.Success };
        }
    }
}
