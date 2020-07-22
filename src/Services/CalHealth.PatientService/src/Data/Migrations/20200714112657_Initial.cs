using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CalHealth.PatientService.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StreetName = table.Column<string>(maxLength: 100, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Town = table.Column<string>(maxLength: 100, nullable: false),
                    State = table.Column<string>(maxLength: 60, nullable: false),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Allergy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(unicode: false, maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Religion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Religion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenderId = table.Column<int>(nullable: false),
                    ReligionId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patient_Gender",
                        column: x => x.GenderId,
                        principalTable: "Gender",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patient_Religion",
                        column: x => x.ReligionId,
                        principalTable: "Religion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PatientAddress",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAddress", x => new { x.PatientId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_PatientAddress_Address",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAddress_Patient",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientAllergy",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    AllergyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergy", x => new { x.PatientId, x.AllergyId });
                    table.ForeignKey(
                        name: "FK_PatientAllergy_Allergy",
                        column: x => x.AllergyId,
                        principalTable: "Allergy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAllergy_Patient",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientPhoneNumber",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false),
                    PhoneNumberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPhoneNumber", x => new { x.PatientId, x.PhoneNumberId });
                    table.ForeignKey(
                        name: "FK_PatientPhoneNumber_Patient",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPhoneNumber_PhoneNumber",
                        column: x => x.PhoneNumberId,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Allergy",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Latex" },
                    { 2, "Nuts" },
                    { 3, "Fruit" },
                    { 4, "Shellfish" },
                    { 5, "Egg" },
                    { 6, "Lactose" },
                    { 7, "Mould" },
                    { 8, "Antibiotics" }
                });

            migrationBuilder.InsertData(
                table: "Gender",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 2, "Female" },
                    { 1, "Male" }
                });

            migrationBuilder.InsertData(
                table: "Religion",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 8, "Hinduism" },
                    { 1, "Christianity (Protestant)" },
                    { 2, "Christianity (Roman Catholic)" },
                    { 3, "Christianity (Orthodox)" },
                    { 4, "Islam (Shia)" },
                    { 5, "Islam (Sunni)" },
                    { 6, "Judaism" },
                    { 7, "Buddhism" },
                    { 9, "Scientology" }
                });

            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "GenderId", "LastName", "ReligionId" },
                values: new object[,]
                {
                    { 4, new DateTime(2007, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daniaal", 1, "Hill", null },
                    { 6, new DateTime(1977, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rhodri", 1, "Ellis", null },
                    { 7, new DateTime(2001, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hakeem", 1, "Conner", null },
                    { 8, new DateTime(1963, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nur", 1, "Lim", null },
                    { 9, new DateTime(1990, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kenzo", 1, "Traynor", null },
                    { 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alima", 2, "Rankin", null },
                    { 2, new DateTime(1980, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chelsie", 2, "Regan", null },
                    { 3, new DateTime(1997, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Michalina", 2, "Dejesus", null },
                    { 5, new DateTime(1989, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adele", 2, "Benjamin", null },
                    { 10, new DateTime(2007, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nyla", 2, "Davey", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patient_GenderId",
                table: "Patient",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_ReligionId",
                table: "Patient",
                column: "ReligionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAddress_AddressId",
                table: "PatientAddress",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergy_AllergyId",
                table: "PatientAllergy",
                column: "AllergyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPhoneNumber_PhoneNumberId",
                table: "PatientPhoneNumber",
                column: "PhoneNumberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientAddress");

            migrationBuilder.DropTable(
                name: "PatientAllergy");

            migrationBuilder.DropTable(
                name: "PatientPhoneNumber");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Allergy");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "Religion");
        }
    }
}
