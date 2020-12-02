using Microsoft.EntityFrameworkCore.Migrations;

namespace Hermes_Services.Migrations
{
    public partial class AddIsConnected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "UsersInGroup",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConnected",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "UsersInGroup");

            migrationBuilder.DropColumn(
                name: "IsConnected",
                table: "AspNetUsers");
        }
    }
}
