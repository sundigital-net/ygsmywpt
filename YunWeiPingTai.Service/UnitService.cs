using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YunWeiPingTai.DTO;
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
            if (id <= 0)
            {
                //查重
                /*
                var exsit = _dbContext.Units.Any(t => t.Name == name);
                if (exsit)
                {
                    return -1;
                }*/
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

        public void UnitDel(long unitId)
        {
            var unit = _dbContext.Units.SingleOrDefault(t => t.Id == unitId);
            if (unit == null)
            { throw new ArgumentException("单位信息不存在：id=" + unitId); }

            unit.IsDeleted = true;
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
            var unitDevice = _dbContext.UnitDevices.SingleOrDefault(t => t.Id == id);
            return unitDevice == null ? null : ToDto(unitDevice);
        }

        public List<UnitDeviceDTO> GetByUnitId(long unitId)
        {
            var unitDevices = _dbContext.UnitDevices.Where(t => t.UnitId == unitId).ToList();
            var list=new List<UnitDeviceDTO>();
            foreach (var unitDevice in unitDevices)
            {
                list.Add(ToDto(unitDevice));
            }

            return list;
        }

        public long AddOrEdit(long unitId, long deviceId, long userId, string snCode)
        {
            var entity=new UnitDeviceEntity()
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

        public void UnitDeviceDel(long unitDeviceId)
        {
            throw new NotImplementedException();
        }
    }
}
