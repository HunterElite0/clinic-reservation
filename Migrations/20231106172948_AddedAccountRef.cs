using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clinic_reservation.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccountRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patient_AccountId",
                table: "Patient",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_AccountId",
                table: "Doctor",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_Account_AccountId",
                table: "Doctor",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Account_AccountId",
                table: "Patient",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_Account_AccountId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Account_AccountId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_AccountId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_AccountId",
                table: "Doctor");
        }
    }
}
