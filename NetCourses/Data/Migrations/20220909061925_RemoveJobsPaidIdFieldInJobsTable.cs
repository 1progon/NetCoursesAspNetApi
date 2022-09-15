using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCourses.DB.Migrations
{
    public partial class RemoveJobsPaidIdFieldInJobsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobsPaidId",
                table: "Jobs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "JobsPaidId",
                table: "Jobs",
                type: "bigint",
                nullable: true);
        }
    }
}
