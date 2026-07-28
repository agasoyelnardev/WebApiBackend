using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class EndChangesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCollectionItems_Books_BookId",
                table: "BookCollectionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCollectionLikes_AspNetUsers_UserId",
                table: "BookCollectionLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCollectionLikes_BookCollections_BookCollectionId1",
                table: "BookCollectionLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionLikes_AspNetUsers_UserId",
                table: "DiscussionLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_AuthorId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCollectionItems_Movies_MovieId",
                table: "MovieCollectionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingProgresses_AspNetUsers_AppUserId",
                table: "ReadingProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedBookCollections_BookCollections_BookCollectionId",
                table: "SavedBookCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedMovieCollections_MovieCollections_MovieCollectionId",
                table: "SavedMovieCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId",
                table: "StreamRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_Movies_MovieId",
                table: "StreamRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBookWatchlistItems_AspNetUsers_UserId",
                table: "UserBookWatchlistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchHistories_AspNetUsers_UserId",
                table: "WatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_WatchHistories_UserId_MovieId",
                table: "WatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_ReadingProgresses_AppUserId",
                table: "ReadingProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_RelatedEntityId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_SenderId",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionLikes_DiscussionId_UserId",
                table: "DiscussionLikes");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionLikes_UserId",
                table: "DiscussionLikes");

            migrationBuilder.DropIndex(
                name: "IX_BookVsMovieVotes_BookVsMovieId_UserId",
                table: "BookVsMovieVotes");

            migrationBuilder.DropIndex(
                name: "IX_BookVsMovieVotes_UserId",
                table: "BookVsMovieVotes");

            migrationBuilder.DropIndex(
                name: "IX_BookReviews_UserId",
                table: "BookReviews");

            migrationBuilder.DropIndex(
                name: "IX_BookCollectionLikes_BookCollectionId1",
                table: "BookCollectionLikes");

            migrationBuilder.DropIndex(
                name: "IX_BookCollectionItems_BookCollectionId",
                table: "BookCollectionItems");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ReadingProgresses");

            migrationBuilder.DropColumn(
                name: "BookCollectionId1",
                table: "BookCollectionLikes");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "StreamRooms",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "StreamRooms",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "StreamUrl",
                table: "StreamRooms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "StreamRooms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CurrentPage",
                table: "ReadingProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Notifications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrailerUrl",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Poster",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalTitle",
                table: "Movies",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ExternalUrl",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Movies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Director",
                table: "Movies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Banner",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsNewRelease",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTopRated",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrending",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Discussions",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000);

            migrationBuilder.AlterColumn<int>(
                name: "Category",
                table: "Discussions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "ChatMessages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserAvatarUrl",
                table: "ChatMessages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MessageText",
                table: "ChatMessages",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BookVsMovies",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BookVsMovies",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PdfUrl",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Books",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "BookReviews",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BookCollections",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BookCollections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "BookCollections",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WatchHistories_UserId",
                table: "WatchHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerId_FollowingId",
                table: "UserFollows",
                columns: new[] { "FollowerId", "FollowingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_MovieId",
                table: "Reviews",
                columns: new[] { "UserId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                table: "Friendships",
                columns: new[] { "SenderId", "ReceiverId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_DiscussionId",
                table: "DiscussionLikes",
                column: "DiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_UserId_DiscussionId",
                table: "DiscussionLikes",
                columns: new[] { "UserId", "DiscussionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookVsMovieVotes_BookVsMovieId",
                table: "BookVsMovieVotes",
                column: "BookVsMovieId");

            migrationBuilder.CreateIndex(
                name: "IX_BookVsMovieVotes_UserId_BookVsMovieId",
                table: "BookVsMovieVotes",
                columns: new[] { "UserId", "BookVsMovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookReviews_UserId_BookId",
                table: "BookReviews",
                columns: new[] { "UserId", "BookId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookCollectionItems_BookCollectionId_BookId",
                table: "BookCollectionItems",
                columns: new[] { "BookCollectionId", "BookId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollectionItems_Books_BookId",
                table: "BookCollectionItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollectionLikes_AspNetUsers_UserId",
                table: "BookCollectionLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionLikes_AspNetUsers_UserId",
                table: "DiscussionLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_AuthorId",
                table: "Discussions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCollectionItems_Movies_MovieId",
                table: "MovieCollectionItems",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedBookCollections_BookCollections_BookCollectionId",
                table: "SavedBookCollections",
                column: "BookCollectionId",
                principalTable: "BookCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedMovieCollections_MovieCollections_MovieCollectionId",
                table: "SavedMovieCollections",
                column: "MovieCollectionId",
                principalTable: "MovieCollections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId",
                table: "StreamRooms",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_Movies_MovieId",
                table: "StreamRooms",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBookWatchlistItems_AspNetUsers_UserId",
                table: "UserBookWatchlistItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WatchHistories_AspNetUsers_UserId",
                table: "WatchHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCollectionItems_Books_BookId",
                table: "BookCollectionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCollectionLikes_AspNetUsers_UserId",
                table: "BookCollectionLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionLikes_AspNetUsers_UserId",
                table: "DiscussionLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_AuthorId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCollectionItems_Movies_MovieId",
                table: "MovieCollectionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedBookCollections_BookCollections_BookCollectionId",
                table: "SavedBookCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedMovieCollections_MovieCollections_MovieCollectionId",
                table: "SavedMovieCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId",
                table: "StreamRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_StreamRooms_Movies_MovieId",
                table: "StreamRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBookWatchlistItems_AspNetUsers_UserId",
                table: "UserBookWatchlistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WatchHistories_AspNetUsers_UserId",
                table: "WatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_WatchHistories_UserId",
                table: "WatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_UserFollows_FollowerId_FollowingId",
                table: "UserFollows");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId_MovieId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionLikes_DiscussionId",
                table: "DiscussionLikes");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionLikes_UserId_DiscussionId",
                table: "DiscussionLikes");

            migrationBuilder.DropIndex(
                name: "IX_BookVsMovieVotes_BookVsMovieId",
                table: "BookVsMovieVotes");

            migrationBuilder.DropIndex(
                name: "IX_BookVsMovieVotes_UserId_BookVsMovieId",
                table: "BookVsMovieVotes");

            migrationBuilder.DropIndex(
                name: "IX_BookReviews_UserId_BookId",
                table: "BookReviews");

            migrationBuilder.DropIndex(
                name: "IX_BookCollectionItems_BookCollectionId_BookId",
                table: "BookCollectionItems");

            migrationBuilder.DropColumn(
                name: "CurrentPage",
                table: "ReadingProgresses");

            migrationBuilder.DropColumn(
                name: "IsNewRelease",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IsTopRated",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "IsTrending",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "StreamRooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "StreamRooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "StreamUrl",
                table: "StreamRooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CoverImageUrl",
                table: "StreamRooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "ReadingProgresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsRead",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Notifications",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrailerUrl",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Poster",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalTitle",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ExternalUrl",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Duration",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Director",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "Banner",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Discussions",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Discussions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserAvatarUrl",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "MessageText",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BookVsMovies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BookVsMovies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "PdfUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "BookReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "BookCollections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BookCollections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Cover",
                table: "BookCollections",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "BookCollectionId1",
                table: "BookCollectionLikes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchHistories_UserId_MovieId",
                table: "WatchHistories",
                columns: new[] { "UserId", "MovieId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFollows_FollowerId",
                table: "UserFollows",
                column: "FollowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingProgresses_AppUserId",
                table: "ReadingProgresses",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_IsRead",
                table: "Notifications",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RelatedEntityId",
                table: "Notifications",
                column: "RelatedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SenderId",
                table: "Friendships",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_DiscussionId_UserId",
                table: "DiscussionLikes",
                columns: new[] { "DiscussionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_UserId",
                table: "DiscussionLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookVsMovieVotes_BookVsMovieId_UserId",
                table: "BookVsMovieVotes",
                columns: new[] { "BookVsMovieId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookVsMovieVotes_UserId",
                table: "BookVsMovieVotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReviews_UserId",
                table: "BookReviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollectionLikes_BookCollectionId1",
                table: "BookCollectionLikes",
                column: "BookCollectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_BookCollectionItems_BookCollectionId",
                table: "BookCollectionItems",
                column: "BookCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollectionItems_Books_BookId",
                table: "BookCollectionItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollectionLikes_AspNetUsers_UserId",
                table: "BookCollectionLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollectionLikes_BookCollections_BookCollectionId1",
                table: "BookCollectionLikes",
                column: "BookCollectionId1",
                principalTable: "BookCollections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_AuthorId",
                table: "Comments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionLikes_AspNetUsers_UserId",
                table: "DiscussionLikes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_AuthorId",
                table: "Discussions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCollectionItems_Movies_MovieId",
                table: "MovieCollectionItems",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingProgresses_AspNetUsers_AppUserId",
                table: "ReadingProgresses",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedBookCollections_BookCollections_BookCollectionId",
                table: "SavedBookCollections",
                column: "BookCollectionId",
                principalTable: "BookCollections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedMovieCollections_MovieCollections_MovieCollectionId",
                table: "SavedMovieCollections",
                column: "MovieCollectionId",
                principalTable: "MovieCollections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_AspNetUsers_CreatedByUserId",
                table: "StreamRooms",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StreamRooms_Movies_MovieId",
                table: "StreamRooms",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBookWatchlistItems_AspNetUsers_UserId",
                table: "UserBookWatchlistItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchHistories_AspNetUsers_UserId",
                table: "WatchHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
