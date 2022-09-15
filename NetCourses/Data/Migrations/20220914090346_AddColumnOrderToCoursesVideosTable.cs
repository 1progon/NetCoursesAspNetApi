using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCourses.DB.Migrations
{
    public partial class AddColumnOrderToCoursesVideosTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CoursesVideos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "CoursesVideos");
        }
    }
}
