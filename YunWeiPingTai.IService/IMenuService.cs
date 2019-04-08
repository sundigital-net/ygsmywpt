using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;
using YunWeiPingTai.DTO.RequestModel;

namespace YunWeiPingTai.IService
{
    public interface IMenuService:IServiceSupport
    {
        MenuDTO[] GetMenusByRoleId(long roleId);
        List<MenuDTO> GetChildListByParentId(long parentId);
        long AddOrEdit(MenuDTO dto);
        void AddMenuIds(long roleId, long[] menuIds);
        MenuDTO[] GetAll();
        MenuDTO[] GetAll(long parentId);
        bool IsExistsName(string name,long id);
        void MarkDelete(long[] ids);
        TableDataModel LoadData(MenuRequestModel model);
    }
}
