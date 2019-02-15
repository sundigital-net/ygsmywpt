using Microsoft.EntityFrameworkCore;
using MyWebApi.Config;

namespace MyWebApi.Entity
{
    //建立MyDbContext并继承DbContext，需要注册到Container中去
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {
            //Code First，如果没有数据库则会创建一个数据库，如果存在，则这句话不起任何作用。
            //Database.EnsureCreated();
            //数据库迁移
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfig());
        }

        //建立一个DbSet<T>的属性，可以用来查询和保存实例（针对DBSet的linq查询语句将被解释为针对数据库的查询）
        public DbSet<ProductEntity> Products { get; set; }
        
    }
}
