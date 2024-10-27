using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamnApp.Migrations
{
    /// <inheritdoc />
    public partial class IntitalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpotId = table.Column<int>(type: "int", nullable: false),
                    BoatType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    MaxSpeedKnop = table.Column<int>(type: "int", nullable: false),
                    DaysInHarbor = table.Column<int>(type: "int", nullable: false),
                    UniqueProperty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccupiedSpots = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boats", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boats");
        }
    }
}
