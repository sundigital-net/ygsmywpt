using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YunWeiPingTai.Service.Migrations
{
    public partial class AddMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNum = table.Column<string>(maxLength: 20, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    PasswordSalt = table.Column<string>(maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 100, nullable: false),
                    LoginErrorTimes = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    LastLoginErrorDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    LogContent = table.Column<string>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    ReadTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_WorkLogs_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkLogReplies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WorkLogId = table.Column<long>(nullable: false),
                    Reply = table.Column<string>(maxLength: 1024, nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    WorkLogEntityId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkLogReplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_WorkLogReplies_T_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_WorkLogReplies_T_WorkLogs_WorkLogEntityId",
                        column: x => x.WorkLogEntityId,
                        principalTable: "T_WorkLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkLogReplies_UserId",
                table: "T_WorkLogReplies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkLogReplies_WorkLogEntityId",
                table: "T_WorkLogReplies",
                column: "WorkLogEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_T_WorkLogs_UserId",
                table: "T_WorkLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_WorkLogReplies");

            migrationBuilder.DropTable(
                name: "T_WorkLogs");

            migrationBuilder.DropTable(
                name: "T_Users");
        }
    }
}
