using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Festival.DAL.Migrations
{
    public partial class ModelsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bands",
                columns: new[] { "Id", "Genre", "LongDescription", "Name", "OriginCountry", "PhotoURL", "ShortDescription" },
                values: new object[] { new Guid("1cb6fb2f-6c01-4bb4-b0b6-cb0d39c75daa"), 12, "The band was formed in 1981 in Los Angeles by vocalist/guitarist James Hetfield and drummer Lars Ulrich, and has been based in San Francisco for most of its career. The band's fast tempos, instrumentals and aggressive musicianship made them one of the founding big four bands of thrash metal, alongside Megadeth, Anthrax and Slayer. Metallica's current lineup comprises founding members and primary songwriters Hetfield and Ulrich, longtime lead guitarist Kirk Hammett, and bassist Robert Trujillo. Guitarist Dave Mustaine (who went on to form Megadeth after being fired from the band) and bassists Ron McGovney, Cliff Burton (who died in a bus accident in Sweden in 1986) and Jason Newsted are former members of the band.", "Metallica", 237, "MetallicaURL", "American heavy metal band" });

            migrationBuilder.InsertData(
                table: "Stages",
                columns: new[] { "Id", "Name", "PhotoURL", "ShortDescription" },
                values: new object[] { new Guid("bdbfaf5b-fe9f-4a01-be3e-5d41b2d5cc5b"), "Stage A", "StageAURL", "Main Stage" });

            migrationBuilder.InsertData(
                table: "Performances",
                columns: new[] { "Id", "BandId", "EndPerformanceTime", "StageId", "StartPerformanceTime" },
                values: new object[] { new Guid("040fc4f3-6f46-40b4-83d8-90833cdf44a8"), new Guid("1cb6fb2f-6c01-4bb4-b0b6-cb0d39c75daa"), new DateTime(2021, 5, 6, 21, 30, 0, 0, DateTimeKind.Unspecified), new Guid("bdbfaf5b-fe9f-4a01-be3e-5d41b2d5cc5b"), new DateTime(2021, 5, 6, 20, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Performances",
                keyColumn: "Id",
                keyValue: new Guid("040fc4f3-6f46-40b4-83d8-90833cdf44a8"));

            migrationBuilder.DeleteData(
                table: "Bands",
                keyColumn: "Id",
                keyValue: new Guid("1cb6fb2f-6c01-4bb4-b0b6-cb0d39c75daa"));

            migrationBuilder.DeleteData(
                table: "Stages",
                keyColumn: "Id",
                keyValue: new Guid("bdbfaf5b-fe9f-4a01-be3e-5d41b2d5cc5b"));
        }
    }
}
