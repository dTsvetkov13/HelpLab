using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservices.Users.Entities.Migrations
{
    public partial class AnswersAndPostsCountAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswersCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostsCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswersCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostsCount",
                table: "AspNetUsers");
        }
    }
}
