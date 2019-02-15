using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service.Config
{
    public class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {

            builder.ToTable("T_Users");
            //builder.HasQueryFilter(t => !t.IsDeleted);
            /*
            builder.HasKey(t => t.Id);//这句可以不写
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            builder.Property(t => t.PhoneNum).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(100);
            builder.Property(t => t.PasswordSalt).IsRequired().HasMaxLength(100);
            builder.Property(t => t.PasswordHash).IsRequired().HasMaxLength(100);*/
        }
    }
}
