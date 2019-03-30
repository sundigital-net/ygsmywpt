using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YunWeiPingTai.Service.Config;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options):base(options)
        {
            //Code First，如果没有数据库则会创建一个数据库，如果存在，则这句话不起任何作用。
            Database.EnsureCreated();
            //数据库迁移
            //Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //为所有继承了baseEntity的类实现全局过滤，主要用于软删除

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(e=>typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).Property<bool>("IsDeleted");
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsDeleted")),
                    Expression.Constant(false));
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }

            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new WorkLogConfig());
            modelBuilder.ApplyConfiguration(new WorkLogReplyConfig());
            modelBuilder.ApplyConfiguration(new UnitConfig());
            modelBuilder.ApplyConfiguration(new DeviceConfig());
            modelBuilder.ApplyConfiguration(new UnitDeviceConfig());
            modelBuilder.ApplyConfiguration(new RoleMenuConfig());
            modelBuilder.ApplyConfiguration(new MenuConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
        }
        //建立一个DbSet<T>的属性，可以用来查询和保存实例（针对DBSet的linq查询语句将被解释为针对数据库的查询）
        public DbSet<UserEntity> AdminUsers { get; set; }
        public DbSet<WorkLogEntity> WorkLogs { get; set; }
        public DbSet<WorkLogReplyEntity> WorkLogReplies { get; set; }
        public DbSet<UnitEntity> Units { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }
        public DbSet<UnitDeviceEntity> UnitDevices { get; set; }
        public DbSet<MenuEntity> Menus { get; set; }
        public DbSet<RoleMenuEntity> RoleMenus { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
    }
}
