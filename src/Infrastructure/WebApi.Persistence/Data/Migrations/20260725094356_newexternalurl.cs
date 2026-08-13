using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class newexternalurl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalUrl",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookCollectionLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookCollectionId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCollectionLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCollectionLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookCollectionLikes_BookCollections_BookCollectionId",
                        column: x => x.BookCollectionId,
                        principalTable: "BookCollections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookCollectionLikes_BookCollections_BookCollectionId1",
                        column: x => x.BookCollectionId1,
                        principalTable: "BookCollections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SavedBookCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedBookCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedBookCollections_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SavedBookCollections_BookCollections_BookCollectionId",
                        column: x => x.BookCollectionId,
                        principalTable: "BookCollections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCollectionLikes_BookCollectionId",
                table: "BookCollectionLikes",
                column: "BookCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollectionLikes_BookCollectionId1",
                table: "BookCollectionLikes",
                column: "BookCollectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollectionLikes_UserId_BookCollectionId",
                table: "BookCollectionLikes",
                columns: new[] { "UserId", "BookCollectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedBookCollections_BookCollectionId",
                table: "SavedBookCollections",
                column: "BookCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedBookCollections_UserId_BookCollectionId",
                table: "SavedBookCollections",
                columns: new[] { "UserId", "BookCollectionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookCollectionLikes");

            migrationBuilder.DropTable(
                name: "SavedBookCollections");

            migrationBuilder.DropColumn(
                name: "ExternalUrl",
                table: "Movies");
        }
    }
}
