﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCourses.DB.Migrations
{
    public partial class AddColumnJobStatusToJobsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Jobs");
        }
    }
}
