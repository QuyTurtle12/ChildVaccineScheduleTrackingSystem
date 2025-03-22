using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedAppointmentAndPackageForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecuteDate",
                table: "Packages");

            migrationBuilder.AddColumn<Guid>(
                name: "PackageId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Appointments");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExecuteDate",
                table: "Packages",
                type: "datetime2",
                nullable: true);
        }
    }
}
