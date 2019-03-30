#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.IService
* 项目描述 ：
* 类 名 称 ：IRoleService
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.IService
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-22 8:21:46
* 更新时间 ：2019-03-22 8:21:46
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.DTO;

namespace YunWeiPingTai.IService
{
    public interface IRoleService:IServiceSupport
    {
        long AddOrEdit(long id,string name,string remark);
        void Delete(long id);
        List<MenuDTO> GetMenusByRoleId(long roleId);
        long[] GetIdsByRoleId(long roleId);
        RoleDTO[] GetAll();
    }
}
