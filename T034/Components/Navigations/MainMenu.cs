using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using T034.Core.DataAccess;
using T034.Core.Entity;
using T034.ViewModel;

namespace T034.Components.Navigations
{
    public class MainMenu : ViewComponent
    {
        private readonly IBaseDb _db;
        
        /// <summary>
        /// Маппер
        /// </summary>
        private readonly IMapper _mapper;

        public MainMenu(IBaseDb db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
               //TODO use Service instead Db
                var items = _db.Where<MenuItem>(m => m.Parent == null);

                var model = new List<MenuItemViewModel>();
                model = _mapper.Map(items, model);

                foreach (var menuItem in model)
                {
                    var subs = _db.Where<MenuItem>(m => m.Parent.Id == menuItem.Id);
                    menuItem.Childs = _mapper.Map<ICollection<MenuItemViewModel>>(subs).OrderBy(m => m.OrderIndex);
                }

                return View(model.OrderBy(m => m.OrderIndex));
            }
            catch (Exception ex)
            {
                return View("ErrorMessage", (object)"Получение списка");
            }
        }
    }
}
