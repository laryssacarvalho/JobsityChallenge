using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobsityChallenge.Chat.Data.Migrations
{
    public partial class ChatrommTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "ChatroomId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Chatrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chatrooms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatroomId",
                table: "Messages",
                column: "ChatroomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chatrooms_ChatroomId",
                table: "Messages",
                column: "ChatroomId",
                principalTable: "Chatrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chatrooms_ChatroomId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Chatrooms");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatroomId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatroomId",
                table: "Messages");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserId",
                table: "Messages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
