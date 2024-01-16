using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomotiveForumSystem.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalLikesCount = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 8192, nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    PostID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParentCommentId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostID",
                        column: x => x.PostID,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, false, "Tuning" },
                    { 2, false, "Engines" },
                    { 3, false, "Suspension" },
                    { 4, false, "Electronics" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "IsBlocked", "IsDeleted", "LastName", "Password", "PhoneNumber", "UserName" },
                values: new object[,]
                {
                    { 1, "john@mail.com", "John", true, false, false, "Smith", "1234", "0888 102 030", "jonkata" },
                    { 2, "steven@mail.com", "Steven", false, false, false, "Solberg", "1020", null, "stevie" },
                    { 3, "ivan@mail.com", "Ivan", false, false, false, "Ivanov", "3344", null, "vanko_54" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryID", "Content", "CreateDate", "IsDeleted", "Title", "TotalLikesCount", "UserID" },
                values: new object[,]
                {
                    { 1, 1, "Step by step tutorial.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8510), false, "This is a post about the tuning of my supra.", 0, 1 },
                    { 2, 2, "Step by step tutorial.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8560), false, "here i will talk about the supra mk4's 2jz engine", 0, 1 },
                    { 3, 2, "Step by step tutorial.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8565), false, "the engine is incredibly small like all japanese engines", 0, 1 },
                    { 4, 3, "Step by step tutorial.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8569), false, "the suspension on this car is not the best but it does the job", 0, 1 },
                    { 5, 4, "Step by step tutorial.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8574), false, "this is a very old car so there's very few electronics on it", 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreateDate", "IsDeleted", "ParentCommentId", "PostID", "UserID" },
                values: new object[,]
                {
                    { 1, "Awesome. I will follow your tutorial to tune my supra.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8616), false, null, 1, 1 },
                    { 2, "Comment number 2 with ensured min length.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8624), false, null, 2, 2 },
                    { 3, "Comment number 3 with ensured min length.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8628), false, null, 3, 3 },
                    { 4, "Comment number 4 with ensured min length.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8633), false, null, 4, 1 },
                    { 5, "Comment number 5 with ensured min length.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8636), false, null, 4, 1 },
                    { 6, "Comment number 6 with ensured min length.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8643), false, null, 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreateDate", "IsDeleted", "ParentCommentId", "PostID", "UserID" },
                values: new object[] { 7, "This is a reply.", new DateTime(2024, 1, 16, 14, 10, 4, 106, DateTimeKind.Local).AddTicks(8648), false, 6, 4, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostID",
                table: "Comments",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserID",
                table: "Comments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PostId",
                table: "Likes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryID",
                table: "Posts",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserID",
                table: "Posts",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
