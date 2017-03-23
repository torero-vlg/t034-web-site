using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace T034.ViewModel
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        [DisplayName("Содержание")]
        public virtual string Body { get; set; }

        [DisplayName("Заголовок")]
        public virtual string Title { get; set; }

        [DisplayName("Краткое описание")]
        public virtual string Resume { get; set; }

        public virtual DateTime LogDate { get; set; }
        public virtual int UserId { get; set; }
        [DisplayName("Автор")]
        public virtual string UserName { get; set; }

        public ICollection<SelectListItem> Newslines { get; set; }

        [DisplayName("Новостная лента")]
        public int NewslineId { get; set; }
        [DisplayName("Новостная лента")]
        public string Newsline { get; set; }
    }
}