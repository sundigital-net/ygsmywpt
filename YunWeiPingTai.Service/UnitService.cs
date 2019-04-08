using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using YunWeiPingTai.DTO;
using YunWeiPingTai.DTO.RequestModel;
using YunWeiPingTai.IService;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    public class UnitService : IUnitService
    {
        private readonly MyDbContext _dbContext;

        public UnitService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public long AddOrEdit(long id,string name, string address, string linkMan, string tel, string phoneNum)
        {
            if (IsExistsName(name, id))
            {
                return -1;
            }
            if (id <= 0)
            {
                var entity = new UnitEntity()
                {
                    Name = name,
                    Address = address,
                    LinkMan = linkMan,
                    Tel = tel,
                    PhoneNum = phoneNum
                };
                _dbContext.Units.Add(entity);
                _dbContext.SaveChanges();
                return entity.Id;
            }
            else
            {
                var unit = _dbContext.Units.FirstOrDefault(t => t.Id == id);
                if (unit == null)
                {
                    throw new ArgumentException("不存在的单位信息，id="+id);
                }

                unit.Name = name;
                unit.Address = address;
                unit.LinkMan = linkMan;
                unit.PhoneNum = phoneNum;
                unit.Tel = tel;
                _dbContext.SaveChanges();
                return id;
            }
        }

        private UnitDTO ToDto(UnitEntity entity)
        {
            var dto = new UnitDTO()
            {
                Name = entity.Name,
                Id = entity.Id,
                CreateTime =entity.CreateTime,
                Address = entity.Address,
                LinkMan = entity.LinkMan,
                Tel = entity.Tel,
                PhoneNum = entity.PhoneNum,
            };
            return dto;
        }

        public List<UnitDTO> GetAll()
        {
            var units = _dbContext.Units.ToList();
            var list=new List<UnitDTO>();
            foreach (var unit in units)
            {
                list.Add(ToDto(unit));
            }

            return list;
        }

        public UnitDTO GetById(long id)
        {
            var unit = _dbContext.Units.SingleOrDefault(t => t.Id == id);
            return unit == null ? null : ToDto(unit);
        }

        public void UnitDel(long[] unitIds)
        {
            if (!unitIds.Any())
            {
                throw new ArgumentException("未选择单位信息");
            }
            var units = _dbContext.Units.Where(t => unitIds.Contains(t.Id)).ToList();
            if (units.Any())
            {
                foreach (var unit in units)
                {
                    unit.IsDeleted = true;
                }
            }
            _dbContext.SaveChanges();
        }

        private UnitDeviceDTO ToDto(UnitDeviceEntity entity)
        {
            var dto=new UnitDeviceDTO()
            {
                AddUserId = entity.AddUserId,
                AddUserName = entity.AddUser.Name,
                CreateTime = entity.CreateTime,
                DeviceId = entity.DeviceId,
                DeviceName = entity.Device.Name,
                DeviceVersion = entity.Device.Version,
                SNCode = entity.SNCode,
                Id=entity.Id,
                UnitId = entity.UnitId,
                UnitName = entity.Unit.Name
            };
            return dto;
        }
        public UnitDeviceDTO GetUnitDevice(long id)
        {
            var unitDevice = _dbContext.UnitDevices.Include(t=>t.Device)
                .Include(t=>t.Unit).Include(t=>t.AddUser).SingleOrDefault(t => t.Id == id);
            return unitDevice == null ? null : ToDto(unitDevice);
        }

        public List<UnitDeviceDTO> GetByUnitId(long unitId)
        {
            var unitDevices = _dbContext.UnitDevices.Include(t => t.Device)
                .Include(t => t.Unit).Include(t => t.AddUser).Where(t => t.UnitId == unitId).ToList();
            var list=new List<UnitDeviceDTO>();
            foreach (var unitDevice in unitDevices)
            {
                list.Add(ToDto(unitDevice));
            }

            return list;
        }

        public long AddOrEdit(long id,long unitId, long deviceId, long userId, string snCode)
        {
            if (_dbContext.UnitDevices.Any(t => t.SNCode == snCode))
            {
                return -1;
            }

            if (id <= 0)//新增
            {
                var entity = new UnitDeviceEntity()
                {
                    AddUserId = userId,
                    DeviceId = deviceId,
                    UnitId = unitId,
                    SNCode = snCode
                };
                _dbContext.UnitDevices.Add(entity);
                _dbContext.SaveChanges();
                return entity.Id;
            }
            else//修改
            {
                var entity = _dbContext.UnitDevices.SingleOrDefault(t => t.Id == id);
                if (entity == null)
                {
                    throw new ArgumentException("不存在的设备信息，Id="+id);
                }

                entity.UnitId = unitId;
                entity.SNCode = snCode;
                entity.DeviceId = deviceId;
                entity.AddUserId = userId;
                _dbContext.SaveChanges();
                return id;
            }
            
        }

        public void UnitDeviceDel(long unitDeviceId)
        {
            throw new NotImplementedException();
        }

        public bool IsExistsName(string name, long id)
        {
            var data = false;
            if (id > 0)
            {
                data = _dbContext.Units.Any(t => t.Id != id && t.Name == name);
            }
            else
            {
                data= _dbContext.Units.Any(t => t.Name == name);
            }

            return data;
        }

        public TableDataModel LoadData(UnitRequestModel model)
        {
            var units = _dbContext.Units.ToList();
            if (!string.IsNullOrEmpty(model.Key))
            {
                units = units.Where(t => t.Name.Contains(model.Key)).ToList();
            }
            List<UnitDTO> list = new List<UnitDTO>();
            foreach (var unit in units)
            {
                list.Add(ToDto(unit));
            }


            var table = new TableDataModel()
            {
                count = list.Count,
                data = list
            };
            return table;
        }

        public TableDataModel LoadData(UnitDeviceRequestModel model)
        {
            var devices = _dbContext.UnitDevices.Include(t => t.Device)
                .Include(t => t.Unit).Include(t => t.AddUser).Where(t=>t.UnitId==model.UnitId).ToList();
            if (!string.IsNullOrEmpty(model.Key))
            {
                devices = devices.Where(t => t.Device.Name.Contains(model.Key)).ToList();
            }
            List<UnitDeviceDTO> list = new List<UnitDeviceDTO>();
            foreach (var device in devices)
            {
                list.Add(ToDto(device));
            }


            var table = new TableDataModel()
            {
                count = list.Count,
                data = list
            };
            return table;
        }
    }
}
