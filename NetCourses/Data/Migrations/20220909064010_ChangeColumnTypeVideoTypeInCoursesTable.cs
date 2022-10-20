using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCourses.DB.Migrations
{
    public partial class ChangeColumnTypeVideoTypeInCoursesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder
                .DropColumn("VideoType", "Courses");

            migrationBuilder.AddColumn<int>(
                name: "VideoType",
                table: "Courses",
                type: "integer",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder
                .DropColumn("VideoType", "Courses");
            
            migrationBuilder.AddColumn<string>(
                name: "VideoType",
                table: "Courses",
                type: "text",
                nullable: true);
  
        }
    }
}
