using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetLedger.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    plate = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    model = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    capacity_kg = table.Column<int>(type: "integer", nullable: false),
                    initial_odometer_km = table.Column<int>(type: "integer", nullable: false),
                    current_odometer_km = table.Column<int>(type: "integer", nullable: false),
                    maintenance_provision_rate_per_km = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    deactivated_at = table.Column<DateTimeOffset>(type: "timestamp(6) with time zone", precision: 6, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp(6) with time zone", precision: 6, nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp(6) with time zone", precision: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicles_organizations_organization_id",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_organization_id",
                table: "vehicles",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_organization_id_plate",
                table: "vehicles",
                columns: new[] { "organization_id", "plate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
