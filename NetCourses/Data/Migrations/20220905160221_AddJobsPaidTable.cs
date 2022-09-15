using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetCourses.DB.Migrations
{
    public partial class AddJobsPaidTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "JobsPaidId",
                table: "Jobs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobsPaid",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BackgroundColor = table.Column<string>(type: "text", nullable: true),
                    ColorBorder = table.Column<string>(type: "text", nullable: true),
                    Homepage = table.Column<bool>(type: "boolean", nullable: false),
                    Topped = table.Column<bool>(type: "boolean", nullable: false),
                    JobId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobsPaid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobsPaid_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobsPaid_JobId",
                table: "JobsPaid",
                column: "JobId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobsPaid");

            migrationBuilder.DropColumn(
                name: "JobsPaidId",
                table: "Jobs");
        }
    }
}
