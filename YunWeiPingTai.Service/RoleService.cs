#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service
* 项目描述 ：
* 类 名 称 ：RoleService
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-22 9:22:31
* 更新时间 ：2019-03-22 9:22:31
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ Hany 2019. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    class RoleService : IRoleService
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<RoleService> _logger;
        public RoleService(MyDbContext dbContext,ILogger<RoleService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public long AddOrEdit(long id, string name, string remark)
        {
            if (id <= 0)//新增
            {
                var entity=new RoleEntity()
                {
                    Name = name,
                    Remark =remark 
                };
                _dbContext.Roles.Add(entity);
                _dbContext.SaveChanges();
                return entity.Id;
            }
            else//编辑
            {
                var role = _dbContext.Roles.FirstOrDefault(t => t.Id == id);
                if (role == null)
                {
                    throw new ArgumentException("不存在的角色，id="+id);
                }
                role.Name = name;
                role.Remark = remark;
                _dbContext.SaveChanges();
                return id;
            }
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        private MenuDTO Todto(MenuEntity entity)
        {
            var dto=new MenuDTO()
            {
                ParentId = entity.ParentId,
                CreateTime = entity.CreateTime,
                DisplayName = entity.DisplayName,
                IconUrl = entity.IconUrl,
                Id=entity.Id,
                LinkUrl = entity.LinkUrl,
                Name = entity.Name,
                Permission = entity.Permission,
                Sort = entity.Sort
            };
            return dto;
        }
        public List<MenuDTO> GetMenusByRoleId(long roleId)
        {
            var ids = _dbContext.RoleMenus.Where(t => t.RoleId == roleId).Select(t=>t.MenuId).ToArray();

            var menus = _dbContext.Menus.Where(t => ids.Contains(t.Id)).ToList();
            var list=new List<MenuDTO>();
            foreach (var item in menus)
            {
                list.Add(Todto(item));
            }

            return list;
        }

        public long[] GetIdsByRoleId(long roleId)
        {
            var ids=_dbContext.RoleMenus.Where(t => t.RoleId == roleId).Select(t => t.MenuId).ToArray();
            return ids;
        }

        public RoleDTO[] GetAll()
        {
            var roles = _dbContext.Roles.ToList();
            var list = new List<RoleDTO>();
            foreach (var role in roles)
            {
                list.Add(Todto(role));
            }

            return list.ToArray();
        }

        private RoleDTO Todto(RoleEntity entity)
        {
            var dto=new RoleDTO()
            {
                CreateTime = entity.CreateTime,
                Id=entity.Id,
                Name = entity.Name,
                Remark = entity.Remark
            };
            return dto;
        }
    }
}
