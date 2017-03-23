using System.Collections.Generic;
using AutoMapper;
using Db.Dto;
using Db.Entity.Administration;
using Db.Services.Common;

namespace Db.Services.Administration
{
    public interface IRoleService : IService
    {
        RoleDto Create(RoleDto dto);
        RoleDto Update(RoleDto dto);
        IEnumerable<RoleDto> Select();
        RoleDto Get(object id);
        OperationResult Delete(object id);
    }

    public class RoleService : AbstractRepository<Role, RoleDto, int>, IRoleService
    {
    }
}
