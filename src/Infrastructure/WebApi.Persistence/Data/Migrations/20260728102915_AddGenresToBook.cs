using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGenresToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genres",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Books");
        }
    }
}
