using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryData2.Migrations
{
    public partial class secondMigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(nullable: true),
                    DayOfWeek = table.Column<int>(nullable: false),
                    OpenDays = table.Column<int>(nullable: false),
                    CloseTimes = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchHours_LibraryBranchs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "LibraryBranchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchHours_BranchId",
                table: "BranchHours",
                column: "BranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchHours");
        }
    }
}
