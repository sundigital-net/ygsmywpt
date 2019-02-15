using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YunWeiPingTai.Service.Migrations
{
    public partial class AddIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Units",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 1024, nullable: false),
                    LinkMan = table.Column<string>(maxLength: 20, nullable: false),
                    Tel = table.Column<string>(maxLength: 20, nullable: true),
                    PhoneNum = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Units", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Units");
        }
    }
}
