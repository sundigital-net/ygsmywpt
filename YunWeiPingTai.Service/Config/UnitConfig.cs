using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using YunWeiPingTai.Service.Entity;

namespace YunWeiPingTai.Service.Config
{
    public class UnitConfig : IEntityTypeConfiguration<UnitEntity>
    {
        public void Configure(EntityTypeBuilder<UnitEntity> builder)
        {
            builder.ToTable("T_Units");
            builder.Property(t => t.Address).IsRequired().HasMaxLength(1024);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.LinkMan).IsRequired().HasMaxLength(20);
            builder.Property(t => t.PhoneNum).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Tel).HasMaxLength(20);
        }
    }
}
