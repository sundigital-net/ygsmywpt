#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：YunWeiPingTai.Service
* 项目描述 ：
* 类 名 称 ：DeviceService
* 类 描 述 ：
* 所在的域 ：HANYZHANG
* 命名空间 ：YunWeiPingTai.Service
* 机器名称 ：HANYZHANG 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：Hany
* 创建时间 ：2019-03-09 9:13:21
* 更新时间 ：2019-03-09 9:13:21
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
using Microsoft.EntityFrameworkCore.Internal;
using YunWeiPingTai.DTO;
using YunWeiPingTai.IService;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    class DeviceService : IDeviceService
    {
        private readonly MyDbContext _dbContext;
        public DeviceService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public long AddNew(string name, string version,string maker)
        {
            var device = _dbContext.Devices.ToList();
            var exist= device.Any(t => t.Name == name && t.Version == version);
            if (exist)
            {
                return -1;//已存在相同的名称和版本型号
            }

            var entity = new DeviceEntity
            {
                Name=name,
                Version = version,
                Maker = maker

            };
            _dbContext.Devices.Add(entity);
            _dbContext.SaveChanges();
            return entity.Id;
        }

        public void Delete(long id)
        {
            var device = _dbContext.Devices.SingleOrDefault(t => t.Id == id);
            if (device == null)
            { throw new ArgumentException("设备信息不存在：id=" + id); }

            device.IsDeleted = true;
            _dbContext.SaveChanges();
        }

        private DeviceDTO Todto(DeviceEntity entity)
        {
            var dto=new DeviceDTO()
            {
                CreateTime = entity.CreateTime,
                Id=entity.Id,
                Name = entity.Name,
                Version = entity.Version,
                Maker=entity.Maker
            };
            return dto;
        }

        public DeviceDTO[] GetAll()
        {
            var devices = _dbContext.Devices.ToList();
            var list = new List<DeviceDTO>();
            foreach (var device in devices)
            {
                list.Add(Todto(device));
            }
            return list.ToArray();
        }

        public DeviceDTO GetById(long id)
        {
            var device = _dbContext.Devices.SingleOrDefault(t => t.Id == id);
            return device == null ? null : Todto(device);
        }
    }
}
