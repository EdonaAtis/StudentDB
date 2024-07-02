using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student.DataModels.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMajorInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MajorInfos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MajorInfos",
                columns: table => new
                {
                    MajorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorInfos", x => x.MajorId);
                });
        }
    }
}
