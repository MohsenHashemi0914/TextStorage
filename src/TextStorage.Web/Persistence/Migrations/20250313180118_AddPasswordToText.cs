using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TextStorage.Web.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Texts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Texts");
        }
    }
}
