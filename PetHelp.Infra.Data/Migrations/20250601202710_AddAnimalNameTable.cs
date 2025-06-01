using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetHelp.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimalNameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AnimalType",
                table: "Animals",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "AnimalName",
                table: "Adoptions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimalName",
                table: "Adoptions");

            migrationBuilder.AlterColumn<int>(
                name: "AnimalType",
                table: "Animals",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
