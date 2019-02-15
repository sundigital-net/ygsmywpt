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
        public long AddNew(string name, string address, string linkMan, string tel, string phoneNum)
        {
            //ToDo 单位名称是否需要查重
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

        public UnitDTO[] GetAll()
        {
            var units = _dbContext.Units.ToList();
            var list=new List<UnitDTO>();
            foreach (var unit in units)
            {
                list.Add(ToDto(unit));
            }

            return list.ToArray();
        }

        public UnitDTO GetById(long id)
        {
            var unit = _dbContext.Units.SingleOrDefault(t => t.Id == id);
            return unit == null ? null : ToDto(unit);
        }
    }
}
