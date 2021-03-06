using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNET_Project_Eleven.Migrations
{
    public partial class ArticleWithCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Articles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Articles");
        }
    }
}
