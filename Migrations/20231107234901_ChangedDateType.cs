using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinic_reservation.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Slot");

            migrationBuilder.AddColumn<string>(
                name: "endTime",
                table: "Slot",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "startTime",
                table: "Slot",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "endTime",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "startTime",
                table: "Slot");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Slot",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
