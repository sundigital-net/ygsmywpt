using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    public class BaseService<T>where T:BaseEntity
    {
        private readonly MyDbContext _dbContext;
        public BaseService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 获取所有没有软删除的数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().Where(e => e.IsDeleted == false);
        }

        /// <summary>
        /// 查找id=id的数据，如果找不到返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById(long id)
        {
            return GetAll().SingleOrDefault(t=>t.Id==id);
        }

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        public void MarkDeleted(long id)
        {
            var data = GetById(id);
            if (data != null)
            {
                data.IsDeleted = true;
                _dbContext.SaveChanges();
            }
        }
    }
}
