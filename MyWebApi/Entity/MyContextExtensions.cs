using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi.Entity
{
    public static class MyContextExtensions//向数据库中添加种子数据
    {
        public static void EnsureSeedDataForContext(this MyDbContext dbContext)
        {
            if(dbContext.Products.Count()>0)
            {
                return;
            }
            var products = new List<ProductEntity>
            {
                new ProductEntity{
                    Name="牛奶",
                    Price=2.5,
                    Description="这是伊利纯牛奶"
                },
                new ProductEntity{
                    Name="啤酒",
                    Price=4,
                    Description="青岛啤酒"
                }
            };
            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }
    }
}
