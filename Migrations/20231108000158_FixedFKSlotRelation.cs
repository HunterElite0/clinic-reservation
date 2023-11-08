using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinic_reservation.Migrations
{
    /// <inheritdoc />
    public partial class FixedFKSlotRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slot_Appointment_AppointmentId",
                table: "Slot");

            migrationBuilder.DropIndex(
                name: "IX_Slot_AppointmentId",
                table: "Slot");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Slot");

            migrationBuilder.AddColumn<int>(
                name: "SlotId",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_SlotId",
                table: "Appointment",
                column: "SlotId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Slot_SlotId",
                table: "Appointment",
                column: "SlotId",
                principalTable: "Slot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Slot_SlotId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_SlotId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Appointment");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Slot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Slot_AppointmentId",
                table: "Slot",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Slot_Appointment_AppointmentId",
                table: "Slot",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
