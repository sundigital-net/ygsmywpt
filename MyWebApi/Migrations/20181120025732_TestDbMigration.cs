using Microsoft.EntityFrameworkCore.Migrations;

namespace MyWebApi.Migrations
{
    public partial class TestDbMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Products",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Products", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Products");
        }
    }
}
