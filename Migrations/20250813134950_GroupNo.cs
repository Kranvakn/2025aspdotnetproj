using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aspApp.Migrations
{
    /// <inheritdoc />
    public partial class GroupNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AddColumn<string>(
                    name: "GroupNo",
                    table: "TodoItems",
                    type: "longtext",
                    nullable: false
                )
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "GroupNo", table: "TodoItems");
        }
    }
}
