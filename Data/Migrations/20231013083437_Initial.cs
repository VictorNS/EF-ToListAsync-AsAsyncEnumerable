using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EF_ToListAsync_AsAsyncEnumerable.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SomeProperty1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeProperty9 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entities");
        }
    }
}
