#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServerManagement.Migrations.ServerManagement;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Servers",
            table => new
            {
                ServerId = table.Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                IsOnline = table.Column<bool>("INTEGER", nullable: false),
                Name = table.Column<string>("TEXT", nullable: false),
                City = table.Column<string>("TEXT", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_Servers", x => x.ServerId); });

        migrationBuilder.InsertData(
            "Servers",
            new[] { "ServerId", "City", "IsOnline", "Name" },
            new object[,]
            {
                { 1, "Toronto", true, "Server1" },
                { 2, "Toronto", true, "Server2" },
                { 3, "Toronto", false, "Server3" },
                { 4, "Toronto", true, "Server4" },
                { 5, "Montreal", false, "Server5" },
                { 6, "Montreal", true, "Server6" },
                { 7, "Montreal", false, "Server7" },
                { 8, "Ottawa", true, "Server8" },
                { 9, "Ottawa", true, "Server9" },
                { 10, "Calgary", true, "Server10" },
                { 11, "Calgary", true, "Server11" },
                { 12, "Halifax", true, "Server12" },
                { 13, "Halifax", false, "Server13" },
                { 14, "Halifax", false, "Server14" },
                { 15, "Halifax", false, "Server15" }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "Servers");
    }
}