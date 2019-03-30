#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service
* 项目描述 ：
* 类 名 称 ：MenuService
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-21 17:49:31
* 更新时间 ：2019-03-21 17:49:31
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
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    class MenuService : IMenuService
    {
        private readonly MyDbContext _dbContext;
        public MenuService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private void DeleteRoleMenus(long roleId)
        {
            var roleMenus = _dbContext.RoleMenus.Include(t => t.Role).Include(t => t.Menu)
                .Where(t => t.RoleId == roleId).ToList();
            if (!roleMenus.Any() || roleMenus[0] == null)
            {
                return;
            }

            foreach (var roleMenu in roleMenus)
            {
                roleMenu.IsDeleted = true;
            }

            _dbContext.SaveChanges();
        }
        public void AddMenuIds(long roleId, long[] menuIds)
        {
            var role = _dbContext.Roles.SingleOrDefault(t => t.Id == roleId);
            if (role == null)
            {
                throw new ArgumentException("roleId不存在:" + roleId);
            }
            DeleteRoleMenus(roleId);
            foreach (var menuId in menuIds)
            {
                var entity=new RoleMenuEntity()
                {
                    RoleId = roleId,
                    MenuId = menuId,
                };
                _dbContext.RoleMenus.Add(entity);
            }

            _dbContext.SaveChanges();
        }

        public long AddOrEdit(MenuDTO dto)
        {
            if (dto.Id <= 0)//新增
            {
                var entity=new MenuEntity()
                {
                    DisplayName = dto.DisplayName,
                    IconUrl = dto.IconUrl,
                    LinkUrl = dto.LinkUrl,
                    Name = dto.Name,
                    ParentId = dto.ParentId,
                    Permission =dto.Permission
                };
                _dbContext.Menus.Add(entity);
                _dbContext.SaveChanges();
                return entity.Id;
            }
            else//编辑
            {
                var entity = _dbContext.Menus.SingleOrDefault(t => t.Id == dto.Id);
                if (entity == null)
                {
                    throw  new ArgumentException("不存在的菜单信息，id="+dto.Id);
                }

                entity.Name = dto.Name;
                entity.DisplayName = dto.DisplayName;
                entity.IconUrl = dto.IconUrl;
                entity.LinkUrl = dto.LinkUrl;
                entity.ParentId = dto.ParentId;
                entity.Permission = dto.Permission;
                entity.Sort = dto.Sort;
                _dbContext.SaveChanges();
                return dto.Id;
            }
        }

        public List<MenuDTO> GetChildListByParentId(long parentId)
        {
            var menus = _dbContext.Menus.ToList();
            if (parentId >= 0)
            {
                menus = menus.Where(t => t.ParentId == parentId).ToList();
            }
            var list = new List<MenuDTO>();
            foreach (var menu in menus)
            {
                list.Add(Todto(menu));
            }

            return list;
        }

        public MenuDTO[] GetMenusByRoleId(long roleId)
        {
            throw new NotImplementedException();
        }

        private MenuDTO Todto(MenuEntity entity)
        {
            var dto=new MenuDTO()
            {
                CreateTime = entity.CreateTime,
                DisplayName = entity.DisplayName,
                IconUrl = entity.IconUrl,
                Id=entity.Id,
                LinkUrl = entity.LinkUrl,
                Name = entity.Name,
                ParentId = entity.ParentId,
                Permission = entity.Permission,
                Sort = entity.Sort
            };
            return dto;
        }

        public MenuDTO[] GetAll()
        {
            var menus = _dbContext.Menus.Include(t => t.ParentMenu).ToList();
            var list=new List<MenuDTO>();
            foreach (var menu in menus)
            {
                list.Add(Todto(menu));
            }

            return list.ToArray();
        }
    }
}
