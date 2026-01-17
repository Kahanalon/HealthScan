using System;
using System.Collections.Generic;
using HealthScan.Domain.Entities.OpenFoodFacts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthScan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOpenFoodFactsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "off_barcode",
                table: "products",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "off_synced_at",
                table: "products",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "off_products",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    product_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    product_name_he = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    product_name_en = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    generic_name = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    generic_name_he = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    generic_name_en = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    brands = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    brands_tags = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    quantity = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    serving_size = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    serving_quantity = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    categories = table.Column<string>(type: "text", nullable: true),
                    categories_tags = table.Column<string>(type: "jsonb", nullable: true),
                    categories_hierarchy = table.Column<string>(type: "text", nullable: true),
                    labels = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    labels_tags = table.Column<string>(type: "jsonb", nullable: true),
                    stores = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    countries = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    countries_tags = table.Column<string>(type: "jsonb", nullable: true),
                    manufacturing_places = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    origins = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    packaging = table.Column<string>(type: "text", nullable: true),
                    packaging_tags = table.Column<string>(type: "jsonb", nullable: true),
                    completeness = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: true),
                    last_modified_t = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_t = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creator = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    editor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    editors_count = table.Column<int>(type: "integer", nullable: true),
                    states = table.Column<string>(type: "text", nullable: true),
                    states_tags = table.Column<string>(type: "jsonb", nullable: true),
                    imported_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    last_synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_off_products", x => x.barcode);
                });

            migrationBuilder.CreateTable(
                name: "off_environment",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    carbon_footprint_100g = table.Column<decimal>(type: "numeric(12,4)", precision: 12, scale: 4, nullable: true),
                    carbon_footprint_serving = table.Column<decimal>(type: "numeric(12,4)", precision: 12, scale: 4, nullable: true),
                    carbon_footprint_unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    carbon_footprint_source = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    environment_impact_level = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    environment_impact_level_tags = table.Column<string>(type: "jsonb", nullable: true),
                    packaging_recycling = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    packaging_components = table.Column<List<OffPackagingComponent>>(type: "jsonb", nullable: true),
                    packaging_materials = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    packaging_materials_tags = table.Column<string>(type: "jsonb", nullable: true),
                    recycling_instruction = table.Column<string>(type: "text", nullable: true),
                    recycling_instruction_to_discard = table.Column<string>(type: "text", nullable: true),
                    recycling_instruction_to_recycle = table.Column<string>(type: "text", nullable: true),
                    origins = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    origins_tags = table.Column<string>(type: "jsonb", nullable: true),
                    manufacturing_places = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    manufacturing_places_tags = table.Column<string>(type: "jsonb", nullable: true),
                    water_footprint_100g = table.Column<decimal>(type: "numeric(12,4)", precision: 12, scale: 4, nullable: true),
                    water_footprint_unit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    agribalyse_food_code = table.Column<int>(type: "integer", nullable: true),
                    agribalyse_food_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    agribalyse_co2_agriculture = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    agribalyse_co2_consumption = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    agribalyse_co2_distribution = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    agribalyse_co2_packaging = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    agribalyse_co2_processing = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    agribalyse_co2_transportation = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    agribalyse_co2_total = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    agribalyse_ef_single_score = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    is_forest_footprint_free = table.Column<bool>(type: "boolean", nullable: true),
                    forest_footprint = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    last_synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_off_environment", x => x.barcode);
                    table.ForeignKey(
                        name: "FK_off_environment_off_products_barcode",
                        column: x => x.barcode,
                        principalTable: "off_products",
                        principalColumn: "barcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "off_images",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_small_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_thumb_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_front_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_front_small_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_front_thumb_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_nutrition_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_nutrition_small_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_nutrition_thumb_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_ingredients_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_ingredients_small_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_ingredients_thumb_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_packaging_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_packaging_small_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    image_packaging_thumb_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    selected_images = table.Column<List<OffImageMetadata>>(type: "jsonb", nullable: true),
                    images_keys = table.Column<string>(type: "jsonb", nullable: true),
                    images_count = table.Column<int>(type: "integer", nullable: true),
                    last_synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_off_images", x => x.barcode);
                    table.ForeignKey(
                        name: "FK_off_images_off_products_barcode",
                        column: x => x.barcode,
                        principalTable: "off_products",
                        principalColumn: "barcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "off_ingredients",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ingredients_text = table.Column<string>(type: "text", nullable: true),
                    ingredients_text_he = table.Column<string>(type: "text", nullable: true),
                    ingredients_text_en = table.Column<string>(type: "text", nullable: true),
                    ingredients_parsed = table.Column<List<OffIngredientItem>>(type: "jsonb", nullable: true),
                    ingredients_count = table.Column<int>(type: "integer", nullable: true),
                    ingredients_percent_analysis = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    allergens = table.Column<string>(type: "jsonb", nullable: true),
                    allergens_tags = table.Column<string>(type: "jsonb", nullable: true),
                    allergens_hierarchy = table.Column<string>(type: "text", nullable: true),
                    traces = table.Column<string>(type: "jsonb", nullable: true),
                    traces_tags = table.Column<string>(type: "jsonb", nullable: true),
                    additives = table.Column<string>(type: "jsonb", nullable: true),
                    additives_tags = table.Column<string>(type: "jsonb", nullable: true),
                    additives_count = table.Column<int>(type: "integer", nullable: true),
                    amino_acids_tags = table.Column<string>(type: "jsonb", nullable: true),
                    minerals_tags = table.Column<string>(type: "jsonb", nullable: true),
                    vitamins_tags = table.Column<string>(type: "jsonb", nullable: true),
                    nucleotides_tags = table.Column<string>(type: "jsonb", nullable: true),
                    other_nutritional_substances_tags = table.Column<string>(type: "jsonb", nullable: true),
                    nova_group = table.Column<int>(type: "integer", nullable: true),
                    nova_groups_markers = table.Column<string>(type: "text", nullable: true),
                    nova_groups_tags = table.Column<string>(type: "jsonb", nullable: true),
                    is_palm_oil_free = table.Column<bool>(type: "boolean", nullable: true),
                    is_vegan = table.Column<bool>(type: "boolean", nullable: true),
                    is_vegetarian = table.Column<bool>(type: "boolean", nullable: true),
                    vegan_analysis = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    vegetarian_analysis = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ingredients_analysis = table.Column<string>(type: "text", nullable: true),
                    ingredients_analysis_tags = table.Column<string>(type: "jsonb", nullable: true),
                    last_synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_off_ingredients", x => x.barcode);
                    table.ForeignKey(
                        name: "FK_off_ingredients_off_products_barcode",
                        column: x => x.barcode,
                        principalTable: "off_products",
                        principalColumn: "barcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "off_nutrition",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    energy_kcal_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    energy_kj_100g = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    energy_kcal_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    energy_kj_serving = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    fat_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    fat_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    saturated_fat_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    saturated_fat_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    monounsaturated_fat_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    polyunsaturated_fat_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    trans_fat_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    cholesterol_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    omega3_fat_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    omega6_fat_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    carbohydrates_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    carbohydrates_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    sugars_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    sugars_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    starch_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    polyols_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    fiber_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    fiber_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    proteins_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    proteins_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    salt_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    salt_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    sodium_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    sodium_serving = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    vitamin_a_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_b1_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_b2_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_b6_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_b9_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_b12_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_c_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_d_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_e_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_k_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    vitamin_pp_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    calcium_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    iron_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    magnesium_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    zinc_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    phosphorus_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    potassium_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    iodine_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    selenium_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    copper_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    manganese_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    fluoride_100g = table.Column<decimal>(type: "numeric(12,6)", precision: 12, scale: 6, nullable: true),
                    caffeine_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    taurine_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    alcohol_100g = table.Column<decimal>(type: "numeric(10,4)", precision: 10, scale: 4, nullable: true),
                    nutrition_data_per = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nutrition_grade_fr = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    last_synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_off_nutrition", x => x.barcode);
                    table.ForeignKey(
                        name: "FK_off_nutrition_off_products_barcode",
                        column: x => x.barcode,
                        principalTable: "off_products",
                        principalColumn: "barcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "off_scores",
                columns: table => new
                {
                    barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nutri_score_grade = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    nutri_score_score = table.Column<int>(type: "integer", nullable: true),
                    nutri_score_version = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nutriscore_grade_2021 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    nutriscore_score_2021 = table.Column<int>(type: "integer", nullable: true),
                    nutriscore_negative_points_2021 = table.Column<int>(type: "integer", nullable: true),
                    nutriscore_positive_points_2021 = table.Column<int>(type: "integer", nullable: true),
                    nutriscore_grade_2023 = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    nutriscore_score_2023 = table.Column<int>(type: "integer", nullable: true),
                    nutriscore_negative_points_2023 = table.Column<int>(type: "integer", nullable: true),
                    nutriscore_positive_points_2023 = table.Column<int>(type: "integer", nullable: true),
                    eco_score_grade = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    eco_score_score = table.Column<int>(type: "integer", nullable: true),
                    eco_score_version = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    eco_score_adjustments = table.Column<int>(type: "integer", nullable: true),
                    eco_score_packaging = table.Column<int>(type: "integer", nullable: true),
                    eco_score_production = table.Column<int>(type: "integer", nullable: true),
                    eco_score_origins = table.Column<int>(type: "integer", nullable: true),
                    eco_score_threatened_species = table.Column<int>(type: "integer", nullable: true),
                    nova_group = table.Column<int>(type: "integer", nullable: true),
                    nutrient_levels_energy = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    nutrient_levels_fat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nutrient_levels_saturated_fat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nutrient_levels_sugars = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nutrient_levels_salt = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    data_quality_errors_count = table.Column<int>(type: "integer", nullable: true),
                    data_quality_errors_tags = table.Column<string>(type: "jsonb", nullable: true),
                    data_quality_warnings_count = table.Column<int>(type: "integer", nullable: true),
                    data_quality_warnings_tags = table.Column<string>(type: "jsonb", nullable: true),
                    data_quality_info_count = table.Column<int>(type: "integer", nullable: true),
                    data_quality_info_tags = table.Column<string>(type: "jsonb", nullable: true),
                    unknown_nutrients_count = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    known_nutrients_count = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    last_synced_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_off_scores", x => x.barcode);
                    table.ForeignKey(
                        name: "FK_off_scores_off_products_barcode",
                        column: x => x.barcode,
                        principalTable: "off_products",
                        principalColumn: "barcode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_off_barcode",
                table: "products",
                column: "off_barcode");

            migrationBuilder.CreateIndex(
                name: "IX_off_environment_carbon_footprint_100g",
                table: "off_environment",
                column: "carbon_footprint_100g");

            migrationBuilder.CreateIndex(
                name: "IX_off_ingredients_is_vegan",
                table: "off_ingredients",
                column: "is_vegan");

            migrationBuilder.CreateIndex(
                name: "IX_off_ingredients_is_vegetarian",
                table: "off_ingredients",
                column: "is_vegetarian");

            migrationBuilder.CreateIndex(
                name: "IX_off_ingredients_nova_group",
                table: "off_ingredients",
                column: "nova_group");

            migrationBuilder.CreateIndex(
                name: "IX_off_products_brands",
                table: "off_products",
                column: "brands");

            migrationBuilder.CreateIndex(
                name: "IX_off_products_completeness",
                table: "off_products",
                column: "completeness");

            migrationBuilder.CreateIndex(
                name: "IX_off_products_last_synced_at",
                table: "off_products",
                column: "last_synced_at");

            migrationBuilder.CreateIndex(
                name: "IX_off_scores_eco_score_grade",
                table: "off_scores",
                column: "eco_score_grade");

            migrationBuilder.CreateIndex(
                name: "IX_off_scores_nova_group",
                table: "off_scores",
                column: "nova_group");

            migrationBuilder.CreateIndex(
                name: "IX_off_scores_nutri_score_grade",
                table: "off_scores",
                column: "nutri_score_grade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "off_environment");

            migrationBuilder.DropTable(
                name: "off_images");

            migrationBuilder.DropTable(
                name: "off_ingredients");

            migrationBuilder.DropTable(
                name: "off_nutrition");

            migrationBuilder.DropTable(
                name: "off_scores");

            migrationBuilder.DropTable(
                name: "off_products");

            migrationBuilder.DropIndex(
                name: "IX_products_off_barcode",
                table: "products");

            migrationBuilder.DropColumn(
                name: "off_barcode",
                table: "products");

            migrationBuilder.DropColumn(
                name: "off_synced_at",
                table: "products");
        }
    }
}
