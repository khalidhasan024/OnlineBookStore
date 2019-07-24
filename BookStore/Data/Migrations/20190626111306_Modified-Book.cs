using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Data.Migrations
{
    public partial class ModifiedBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Books",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Books",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Books",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
