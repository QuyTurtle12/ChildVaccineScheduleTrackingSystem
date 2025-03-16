using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineRecords_Children_childId",
                table: "VaccineRecords");

            migrationBuilder.RenameColumn(
                name: "childId",
                table: "VaccineRecords",
                newName: "ChildId");

            migrationBuilder.RenameIndex(
                name: "IX_VaccineRecords_childId",
                table: "VaccineRecords",
                newName: "IX_VaccineRecords_ChildId");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Packages",
                newName: "Type");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppointmentId",
                table: "Packages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "AppointmentPackage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentPackage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentPackage_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentPackage_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPackage_AppointmentId",
                table: "AppointmentPackage",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPackage_PackageId",
                table: "AppointmentPackage",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineRecords_Children_ChildId",
                table: "VaccineRecords",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VaccineRecords_Children_ChildId",
                table: "VaccineRecords");

            migrationBuilder.DropTable(
                name: "AppointmentPackage");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "VaccineRecords",
                newName: "childId");

            migrationBuilder.RenameIndex(
                name: "IX_VaccineRecords_ChildId",
                table: "VaccineRecords",
                newName: "IX_VaccineRecords_childId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Packages",
                newName: "type");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppointmentId",
                table: "Packages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VaccineRecords_Children_childId",
                table: "VaccineRecords",
                column: "childId",
                principalTable: "Children",
                principalColumn: "Id");
        }
    }
}
