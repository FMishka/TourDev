using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourDev.Migrations
{
    public partial class Some : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManNew",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                });
            migrationBuilder.AddColumn<string>(
                name: "SomeThing",
                table: "Man",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Man");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Man");
            migrationBuilder.DropTable(
                name:"ManNew");
        }
    }
}
