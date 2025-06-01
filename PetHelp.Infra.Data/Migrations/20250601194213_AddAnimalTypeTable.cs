using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHelp.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimalTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimalType",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalType",
                table: "Animals");
        }
    }
}
