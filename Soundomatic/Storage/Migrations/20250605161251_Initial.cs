using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soundomatic.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoundPacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    IconData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    IconMimeType = table.Column<string>(type: "TEXT", nullable: true),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoundPacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeyBindings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<ushort>(type: "INTEGER", nullable: false),
                    PackId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyBindings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyBindings_SoundPacks_PackId",
                        column: x => x.PackId,
                        principalTable: "SoundPacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Sounds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    AudioFormat = table.Column<int>(type: "INTEGER", nullable: false),
                    FileSizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    Volume = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 100),
                    SoundPackId = table.Column<long>(type: "INTEGER", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sounds_SoundPacks_SoundPackId",
                        column: x => x.SoundPackId,
                        principalTable: "SoundPacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyBindings_PackId",
                table: "KeyBindings",
                column: "PackId");

            migrationBuilder.CreateIndex(
                name: "IX_Sounds_SoundPackId",
                table: "Sounds",
                column: "SoundPackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyBindings");

            migrationBuilder.DropTable(
                name: "Sounds");

            migrationBuilder.DropTable(
                name: "SoundPacks");
        }
    }
}
