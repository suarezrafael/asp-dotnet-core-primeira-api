using Microsoft.EntityFrameworkCore.Migrations;

namespace Cities.API.Migrations
{
    public partial class CityInfoDBUpdatePoINomeForName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "PointsOfInterest",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PointsOfInterest",
                newName: "Nome");
        }
    }
}
