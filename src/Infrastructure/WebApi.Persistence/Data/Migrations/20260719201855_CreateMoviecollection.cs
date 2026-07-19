using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMoviecollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "MovieIds",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "MovieCollections");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MovieCollections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "MovieCollections",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "MovieCollections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "MovieCollections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MovieCollections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MovieCollectionItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovieId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCollectionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieCollectionItems_MovieCollections_MovieCollectionId",
                        column: x => x.MovieCollectionId,
                        principalTable: "MovieCollections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieCollectionItems_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieCollectionItems_Movies_MovieId1",
                        column: x => x.MovieId1,
                        principalTable: "Movies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieCollections_AppUserId",
                table: "MovieCollections",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCollectionItems_MovieCollectionId_MovieId",
                table: "MovieCollectionItems",
                columns: new[] { "MovieCollectionId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieCollectionItems_MovieId",
                table: "MovieCollectionItems",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCollectionItems_MovieId1",
                table: "MovieCollectionItems",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCollections_AspNetUsers_AppUserId",
                table: "MovieCollections",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCollections_AspNetUsers_AppUserId",
                table: "MovieCollections");

            migrationBuilder.DropTable(
                name: "MovieCollectionItems");

            migrationBuilder.DropIndex(
                name: "IX_MovieCollections_AppUserId",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "MovieCollections");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MovieCollections");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MovieCollections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "MovieCollections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "MovieCollections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MovieIds",
                table: "MovieCollections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MovieCollections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MovieCollections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "MovieCollections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
