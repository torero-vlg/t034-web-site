using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Db.DataAccess;
using Db.Dto;
using Db.Entity;
using Db.Services.Common;
using Ninject;

namespace Db.Services
{
    public interface IMenuItemService : IService
    {
        IEnumerable<MenuItemDto> Select();
        MenuItemDto ByUrl(string url);
        OperationResult Delete(int menuItemId);
    }

    public class MenuItemService : IMenuItemService
    {
        [Inject]
        public IBaseDb Db { get; set; }

        public IEnumerable<MenuItemDto> Select()
        {
            var list = new List<MenuItemDto>();
            var items = Db.Select<MenuItem>();
            list = Mapper.Map(items, list);
            return list;
        }

        public MenuItemDto ByUrl(string url)
        {
            var item = Db.Where<MenuItem>(i => i.Url == url).FirstOrDefault();
            var dto = Mapper.Map<MenuItemDto>(item);
            return dto;
        }

        public OperationResult Delete(int menuItemId)
        {
            if(Db.Where<MenuItem>(m => m.Parent.Id == menuItemId).Any())
                return new OperationResult { Status = StatusOperation.Error, Message = "Существует пункт меню, у которого удаляемый является родителем"};

            var item = Db.Get<MenuItem>(menuItemId);
            var result = Db.Delete(item);
            return new OperationResult { Status = StatusOperation.Success };
        }
    }

    public class OperationResult
    {
        public StatusOperation Status { get; set; }
        public string Message { get; set; }
    }
    public enum StatusOperation
    {
        Success,
        Error
    }
}
