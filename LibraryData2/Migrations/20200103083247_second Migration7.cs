using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryData2.Migrations
{
    public partial class secondMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchHours_LibraryBranchs_BranchId",
                table: "BranchHours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BranchHours",
                table: "BranchHours");

            migrationBuilder.DropColumn(
                name: "CloseTimes",
                table: "BranchHours");

            migrationBuilder.DropColumn(
                name: "OpenDays",
                table: "BranchHours");

            migrationBuilder.RenameTable(
                name: "BranchHours",
                newName: "BranchHourss");

            migrationBuilder.RenameIndex(
                name: "IX_BranchHours_BranchId",
                table: "BranchHourss",
                newName: "IX_BranchHourss_BranchId");

            migrationBuilder.AddColumn<int>(
                name: "CloseTime",
                table: "BranchHourss",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpenTime",
                table: "BranchHourss",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BranchHourss",
                table: "BranchHourss",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchHourss_LibraryBranchs_BranchId",
                table: "BranchHourss",
                column: "BranchId",
                principalTable: "LibraryBranchs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchHourss_LibraryBranchs_BranchId",
                table: "BranchHourss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BranchHourss",
                table: "BranchHourss");

            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "BranchHourss");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "BranchHourss");

            migrationBuilder.RenameTable(
                name: "BranchHourss",
                newName: "BranchHours");

            migrationBuilder.RenameIndex(
                name: "IX_BranchHourss_BranchId",
                table: "BranchHours",
                newName: "IX_BranchHours_BranchId");

            migrationBuilder.AddColumn<int>(
                name: "CloseTimes",
                table: "BranchHours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OpenDays",
                table: "BranchHours",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BranchHours",
                table: "BranchHours",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchHours_LibraryBranchs_BranchId",
                table: "BranchHours",
                column: "BranchId",
                principalTable: "LibraryBranchs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
