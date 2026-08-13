using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedByUserIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId1",
                table: "StreamRooms");

            migrationBuilder.DropIndex(
                name: "IX_StreamRooms_CreatedByUserId1",
                table: "StreamRooms");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "StreamRooms");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "StreamRooms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_StreamRooms_CreatedByUserId",
                table: "StreamRooms",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId",
                table: "StreamRooms",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId",
                table: "StreamRooms");

            migrationBuilder.DropIndex(
                name: "IX_StreamRooms_CreatedByUserId",
                table: "StreamRooms");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedByUserId",
                table: "StreamRooms",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId1",
                table: "StreamRooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StreamRooms_CreatedByUserId1",
                table: "StreamRooms",
                column: "CreatedByUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId1",
                table: "StreamRooms",
                column: "CreatedByUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
