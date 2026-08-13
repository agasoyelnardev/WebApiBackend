using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByAndMovieToStreamRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "StreamRooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId1",
                table: "StreamRooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "StreamRooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamRooms_CreatedByUserId1",
                table: "StreamRooms",
                column: "CreatedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_StreamRooms_MovieId",
                table: "StreamRooms",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId1",
                table: "StreamRooms",
                column: "CreatedByUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_Movies_MovieId",
                table: "StreamRooms",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId1",
                table: "StreamRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_Movies_MovieId",
                table: "StreamRooms");

            migrationBuilder.DropIndex(
                name: "IX_StreamRooms_CreatedByUserId1",
                table: "StreamRooms");

            migrationBuilder.DropIndex(
                name: "IX_StreamRooms_MovieId",
                table: "StreamRooms");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "StreamRooms");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "StreamRooms");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "StreamRooms");
        }
    }
}
