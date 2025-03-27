using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantAppointmentAndPackageForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentPackage_Appointments_AppointmentId",
                table: "AppointmentPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentPackage_Packages_PackageId",
                table: "AppointmentPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Appointments_AppointmentId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_AppointmentId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Appointments");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentPackage_Appointments_AppointmentId",
                table: "AppointmentPackage",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentPackage_Packages_PackageId",
                table: "AppointmentPackage",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentPackage_Appointments_AppointmentId",
                table: "AppointmentPackage");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentPackage_Packages_PackageId",
                table: "AppointmentPackage");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_AppointmentId",
                table: "Packages",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentPackage_Appointments_AppointmentId",
                table: "AppointmentPackage",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentPackage_Packages_PackageId",
                table: "AppointmentPackage",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
