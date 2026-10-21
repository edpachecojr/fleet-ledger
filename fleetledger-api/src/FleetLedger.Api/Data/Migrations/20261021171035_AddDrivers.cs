using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetLedger.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDrivers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    cpf = table.Column<string>(type: "character(11)", fixedLength: true, maxLength: 11, nullable: false),
                    phone = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    cnh_number = table.Column<string>(type: "character(11)", fixedLength: true, maxLength: 11, nullable: false),
                    cnh_category = table.Column<int>(type: "integer", nullable: false),
                    cnh_expires_on = table.Column<DateOnly>(type: "date", nullable: false),
                    toxicology_exam_expires_on = table.Column<DateOnly>(type: "date", nullable: true),
                    default_commission_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    bank_details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    deactivated_at = table.Column<DateTimeOffset>(type: "timestamp(6) with time zone", precision: 6, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp(6) with time zone", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_drivers", x => x.id);
                    table.ForeignKey(
                        name: "fk_drivers_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_drivers_organization_id",
                table: "drivers",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_drivers_organization_id_cnh_number",
                table: "drivers",
                columns: new[] { "organization_id", "cnh_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_drivers_organization_id_cpf",
                table: "drivers",
                columns: new[] { "organization_id", "cpf" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "drivers");
        }
    }
}
