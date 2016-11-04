using System.Collections.Generic;
using AutoMapper;
using Db.DataAccess;
using Db.Dto;
using Db.Entity.Administration;
using Db.Services.Common;
using Ninject;

namespace Db.Services.Administration
{
    public interface IRoleService : IService
    {
        Role Create(RoleDto dto);
        Role Update(UserDto dto);
        IEnumerable<RoleDto> Select();
        RoleDto Get(int id);
    }

    public class RoleService : IRoleService
    {
        [Inject]
        public IBaseDb Db { get; set; }

        public Role Create(RoleDto dto)
        {
            throw new System.NotImplementedException();
        }

        public Role Update(UserDto dto)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RoleDto> Select()
        {
            var list = new List<RoleDto>();
            var items = Db.Select<Role>();
            list = Mapper.Map(items, list);
            return list;
        }

        public RoleDto Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
