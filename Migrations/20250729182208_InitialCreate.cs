using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarWarsAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pilots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Starships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Manufacturer = table.Column<string>(type: "TEXT", nullable: false),
                    CostInCredits = table.Column<string>(type: "TEXT", nullable: false),
                    Length = table.Column<string>(type: "TEXT", nullable: false),
                    MaxAtmospheringSpeed = table.Column<string>(type: "TEXT", nullable: false),
                    Crew = table.Column<string>(type: "TEXT", nullable: false),
                    Passengers = table.Column<string>(type: "TEXT", nullable: false),
                    CargoCapacity = table.Column<string>(type: "TEXT", nullable: false),
                    Consumables = table.Column<string>(type: "TEXT", nullable: false),
                    HyperdriveRating = table.Column<string>(type: "TEXT", nullable: false),
                    MGLT = table.Column<string>(type: "TEXT", nullable: false),
                    StarshipClass = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Edited = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Starships", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StarshipFilms",
                columns: table => new
                {
                    FilmsId = table.Column<int>(type: "INTEGER", nullable: false),
                    StarshipsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarshipFilms", x => new { x.FilmsId, x.StarshipsId });
                    table.ForeignKey(
                        name: "FK_StarshipFilms_Films_FilmsId",
                        column: x => x.FilmsId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StarshipFilms_Starships_StarshipsId",
                        column: x => x.StarshipsId,
                        principalTable: "Starships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StarshipPilots",
                columns: table => new
                {
                    PilotsId = table.Column<int>(type: "INTEGER", nullable: false),
                    StarshipsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarshipPilots", x => new { x.PilotsId, x.StarshipsId });
                    table.ForeignKey(
                        name: "FK_StarshipPilots_Pilots_PilotsId",
                        column: x => x.PilotsId,
                        principalTable: "Pilots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StarshipPilots_Starships_StarshipsId",
                        column: x => x.StarshipsId,
                        principalTable: "Starships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_Id",
                table: "Films",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pilots_Id",
                table: "Pilots",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StarshipFilms_StarshipsId",
                table: "StarshipFilms",
                column: "StarshipsId");

            migrationBuilder.CreateIndex(
                name: "IX_StarshipPilots_StarshipsId",
                table: "StarshipPilots",
                column: "StarshipsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StarshipFilms");

            migrationBuilder.DropTable(
                name: "StarshipPilots");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "Pilots");

            migrationBuilder.DropTable(
                name: "Starships");
        }
    }
}
