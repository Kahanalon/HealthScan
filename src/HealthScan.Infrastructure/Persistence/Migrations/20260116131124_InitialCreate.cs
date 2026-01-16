using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthScan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ingredient_flags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ingredient_pattern = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    flag_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    penalty_points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    description_he = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description_en = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient_flags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    barcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    name_he = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    name_en = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    brand = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    package_size = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    category = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    energy_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    fat_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    saturated_fat_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    carbohydrates_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    sugars_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    fiber_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    protein_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    sodium_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    serving_size = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    energy_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    fat_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    saturated_fat_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    carbohydrates_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    sugars_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    fiber_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    protein_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    sodium_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    ingredients_text_he = table.Column<string>(type: "text", nullable: true),
                    ingredients_text_en = table.Column<string>(type: "text", nullable: true),
                    ingredients_parsed = table.Column<List<string>>(type: "jsonb", nullable: true),
                    allergens = table.Column<List<string>>(type: "jsonb", nullable: true),
                    image_front_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_nutrition_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_ingredients_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    source = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "user"),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nutrition_complete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scan_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    barcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    device_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    scan_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    score = table.Column<int>(type: "integer", nullable: true),
                    grade = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    scanned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scan_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scoring_rules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rule_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    rule_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    condition_json = table.Column<string>(type: "jsonb", nullable: false),
                    points = table.Column<int>(type: "integer", nullable: false),
                    description_he = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description_en = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scoring_rules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_contributions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    barcode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    field_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    field_value = table.Column<string>(type: "text", nullable: true),
                    image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    device_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_contributions", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_contributions_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ingredient_flags_flag_type",
                table: "ingredient_flags",
                column: "flag_type");

            migrationBuilder.CreateIndex(
                name: "IX_product_contributions_barcode",
                table: "product_contributions",
                column: "barcode");

            migrationBuilder.CreateIndex(
                name: "IX_product_contributions_product_id",
                table: "product_contributions",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_contributions_status",
                table: "product_contributions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_products_barcode",
                table: "products",
                column: "barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_status",
                table: "products",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_scan_events_barcode",
                table: "scan_events",
                column: "barcode");

            migrationBuilder.CreateIndex(
                name: "IX_scan_events_scanned_at",
                table: "scan_events",
                column: "scanned_at");

            migrationBuilder.CreateIndex(
                name: "IX_scoring_rules_rule_name",
                table: "scoring_rules",
                column: "rule_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ingredient_flags");

            migrationBuilder.DropTable(
                name: "product_contributions");

            migrationBuilder.DropTable(
                name: "scan_events");

            migrationBuilder.DropTable(
                name: "scoring_rules");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
