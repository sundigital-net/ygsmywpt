using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.IService
{
    public interface IMenuService:IServiceSupport
    {
        MenuDTO[] GetMenusByRoleId(long roleId);
        List<MenuDTO> GetChildListByParentId(long parentId);
        long AddOrEdit(MenuDTO dto);
        void AddMenuIds(long roleId, long[] menuIds);
        MenuDTO[] GetAll();
    }
}
