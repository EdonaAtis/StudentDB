using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminInfoPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "StudentInfo");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "AdminInfo",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AdminInfo",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminInfo",
                table: "AdminInfo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentInfo",
                table: "StudentInfo",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminInfo",
                table: "AdminInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentInfo",
                table: "StudentInfo");

            migrationBuilder.RenameTable(
                name: "StudentInfo",
                newName: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "AdminInfo",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AdminInfo",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }
    }
}
