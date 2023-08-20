using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task6.Migrations
{
    /// <inheritdoc />
    public partial class DbSetForMessageTagsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageTag_Messages_MessageId",
                table: "MessageTag");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageTag_Tags_TagId",
                table: "MessageTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageTag",
                table: "MessageTag");

            migrationBuilder.RenameTable(
                name: "MessageTag",
                newName: "MessageTags");

            migrationBuilder.RenameIndex(
                name: "IX_MessageTag_TagId",
                table: "MessageTags",
                newName: "IX_MessageTags_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageTags",
                table: "MessageTags",
                columns: new[] { "MessageId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTags_Messages_MessageId",
                table: "MessageTags",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTags_Tags_TagId",
                table: "MessageTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageTags_Messages_MessageId",
                table: "MessageTags");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageTags_Tags_TagId",
                table: "MessageTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageTags",
                table: "MessageTags");

            migrationBuilder.RenameTable(
                name: "MessageTags",
                newName: "MessageTag");

            migrationBuilder.RenameIndex(
                name: "IX_MessageTags_TagId",
                table: "MessageTag",
                newName: "IX_MessageTag_TagId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageTag",
                table: "MessageTag",
                columns: new[] { "MessageId", "TagId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTag_Messages_MessageId",
                table: "MessageTag",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTag_Tags_TagId",
                table: "MessageTag",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
