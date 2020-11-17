using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes_Services.Data.Migrations
{
    public partial class ChangeGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Group",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UsersInGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersInGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersInGroup_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Group_AppUserId",
                table: "Group",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroup_GroupId",
                table: "UsersInGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroup_UserId",
                table: "UsersInGroup",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Group_AspNetUsers_AppUserId",
                table: "Group",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_AspNetUsers_AppUserId",
                table: "Group");

            migrationBuilder.DropTable(
                name: "UsersInGroup");

            migrationBuilder.DropIndex(
                name: "IX_Group_AppUserId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Group");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
