using System.Collections.Generic;
using AutoMapper;
using T034.Core.Dto;
using T034.Core.Entity.Administration;
using T034.Core.Services.Common;

namespace T034.Core.Services.Administration
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
