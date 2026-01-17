using System.Text.Json.Serialization;

namespace HealthScan.Infrastructure.ExternalServices;

public class OffFullResponse
{
    public int Status { get; set; }
    public OffFullProduct? Product { get; set; }
}

public class OffFullSearchResponse
{
    public List<OffFullProduct>? Products { get; set; }
    public int Count { get; set; }
    public int Page { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("page_count")]
    public int PageCount { get; set; }
}

public class OffFullProduct
{
    public string? Code { get; set; }

    [JsonPropertyName("product_name")]
    public string? ProductName { get; set; }

    [JsonPropertyName("product_name_he")]
    public string? ProductNameHe { get; set; }

    [JsonPropertyName("product_name_en")]
    public string? ProductNameEn { get; set; }

    [JsonPropertyName("generic_name")]
    public string? GenericName { get; set; }

    [JsonPropertyName("generic_name_he")]
    public string? GenericNameHe { get; set; }

    [JsonPropertyName("generic_name_en")]
    public string? GenericNameEn { get; set; }

    public string? Brands { get; set; }

    [JsonPropertyName("brands_tags")]
    public List<string>? BrandsTags { get; set; }

    public string? Quantity { get; set; }

    [JsonPropertyName("serving_size")]
    public string? ServingSize { get; set; }

    [JsonPropertyName("serving_quantity")]
    public decimal? ServingQuantity { get; set; }

    public string? Categories { get; set; }

    [JsonPropertyName("categories_tags")]
    public List<string>? CategoriesTags { get; set; }

    [JsonPropertyName("categories_hierarchy")]
    public List<string>? CategoriesHierarchy { get; set; }

    public string? Labels { get; set; }

    [JsonPropertyName("labels_tags")]
    public List<string>? LabelsTags { get; set; }

    public string? Stores { get; set; }
    public string? Countries { get; set; }

    [JsonPropertyName("countries_tags")]
    public List<string>? CountriesTags { get; set; }

    [JsonPropertyName("manufacturing_places")]
    public string? ManufacturingPlaces { get; set; }

    public string? Origins { get; set; }
    public string? Packaging { get; set; }

    [JsonPropertyName("packaging_tags")]
    public List<string>? PackagingTags { get; set; }

    public decimal? Completeness { get; set; }

    [JsonPropertyName("last_modified_t")]
    public long? LastModifiedT { get; set; }

    [JsonPropertyName("created_t")]
    public long? CreatedT { get; set; }

    public string? Creator { get; set; }

    [JsonPropertyName("last_editor")]
    public string? LastEditor { get; set; }

    [JsonPropertyName("editors_tags")]
    public List<string>? EditorsTags { get; set; }

    public string? States { get; set; }

    [JsonPropertyName("states_tags")]
    public List<string>? StatesTags { get; set; }

    [JsonPropertyName("ingredients_text")]
    public string? IngredientsText { get; set; }

    [JsonPropertyName("ingredients_text_he")]
    public string? IngredientsTextHe { get; set; }

    [JsonPropertyName("ingredients_text_en")]
    public string? IngredientsTextEn { get; set; }

    public List<OffFullIngredient>? Ingredients { get; set; }

    [JsonPropertyName("ingredients_n")]
    public decimal? IngredientsN { get; set; }

    [JsonPropertyName("ingredients_percent_analysis")]
    public decimal? IngredientsPercentAnalysis { get; set; }

    [JsonPropertyName("allergens")]
    public string? Allergens { get; set; }

    [JsonPropertyName("allergens_tags")]
    public List<string>? AllergensTags { get; set; }

    [JsonPropertyName("allergens_hierarchy")]
    public List<string>? AllergensHierarchy { get; set; }

    public string? Traces { get; set; }

    [JsonPropertyName("traces_tags")]
    public List<string>? TracesTags { get; set; }

    [JsonPropertyName("additives_tags")]
    public List<string>? AdditivesTags { get; set; }

    [JsonPropertyName("additives_n")]
    public decimal? AdditivesN { get; set; }

    [JsonPropertyName("amino_acids_tags")]
    public List<string>? AminoAcidsTags { get; set; }

    [JsonPropertyName("minerals_tags")]
    public List<string>? MineralsTags { get; set; }

    [JsonPropertyName("vitamins_tags")]
    public List<string>? VitaminsTags { get; set; }

    [JsonPropertyName("nucleotides_tags")]
    public List<string>? NucleotidesTags { get; set; }

    [JsonPropertyName("other_nutritional_substances_tags")]
    public List<string>? OtherNutritionalSubstancesTags { get; set; }

    [JsonPropertyName("nova_group")]
    public decimal? NovaGroup { get; set; }

    [JsonPropertyName("nova_groups_markers")]
    public object? NovaGroupsMarkers { get; set; }

    [JsonPropertyName("nova_groups_tags")]
    public List<string>? NovaGroupsTags { get; set; }

    [JsonPropertyName("ingredients_analysis_tags")]
    public List<string>? IngredientsAnalysisTags { get; set; }

    public OffFullNutriments? Nutriments { get; set; }

    [JsonPropertyName("nutrition_data_per")]
    public string? NutritionDataPer { get; set; }

    [JsonPropertyName("nutrition_grade_fr")]
    public string? NutritionGradeFr { get; set; }

    [JsonPropertyName("nutriscore_grade")]
    public string? NutriscoreGrade { get; set; }

    [JsonPropertyName("nutriscore_score")]
    public decimal? NutriscoreScore { get; set; }

    [JsonPropertyName("nutriscore_version")]
    public string? NutriscoreVersion { get; set; }

    [JsonPropertyName("nutriscore_2021_tags")]
    public List<string>? Nutriscore2021Tags { get; set; }

    [JsonPropertyName("nutriscore_2023_tags")]
    public List<string>? Nutriscore2023Tags { get; set; }

    [JsonPropertyName("ecoscore_grade")]
    public string? EcoscoreGrade { get; set; }

    [JsonPropertyName("ecoscore_score")]
    public decimal? EcoscoreScore { get; set; }

    [JsonPropertyName("ecoscore_tags")]
    public List<string>? EcoscoreTags { get; set; }

    [JsonPropertyName("nutrient_levels")]
    public OffNutrientLevels? NutrientLevels { get; set; }

    [JsonPropertyName("data_quality_errors_tags")]
    public List<string>? DataQualityErrorsTags { get; set; }

    [JsonPropertyName("data_quality_warnings_tags")]
    public List<string>? DataQualityWarningsTags { get; set; }

    [JsonPropertyName("data_quality_info_tags")]
    public List<string>? DataQualityInfoTags { get; set; }

    [JsonPropertyName("unknown_nutrients_tags")]
    public List<string>? UnknownNutrientsTags { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("image_small_url")]
    public string? ImageSmallUrl { get; set; }

    [JsonPropertyName("image_thumb_url")]
    public string? ImageThumbUrl { get; set; }

    [JsonPropertyName("image_front_url")]
    public string? ImageFrontUrl { get; set; }

    [JsonPropertyName("image_front_small_url")]
    public string? ImageFrontSmallUrl { get; set; }

    [JsonPropertyName("image_front_thumb_url")]
    public string? ImageFrontThumbUrl { get; set; }

    [JsonPropertyName("image_nutrition_url")]
    public string? ImageNutritionUrl { get; set; }

    [JsonPropertyName("image_nutrition_small_url")]
    public string? ImageNutritionSmallUrl { get; set; }

    [JsonPropertyName("image_nutrition_thumb_url")]
    public string? ImageNutritionThumbUrl { get; set; }

    [JsonPropertyName("image_ingredients_url")]
    public string? ImageIngredientsUrl { get; set; }

    [JsonPropertyName("image_ingredients_small_url")]
    public string? ImageIngredientsSmallUrl { get; set; }

    [JsonPropertyName("image_ingredients_thumb_url")]
    public string? ImageIngredientsThumbUrl { get; set; }

    [JsonPropertyName("image_packaging_url")]
    public string? ImagePackagingUrl { get; set; }

    [JsonPropertyName("image_packaging_small_url")]
    public string? ImagePackagingSmallUrl { get; set; }

    [JsonPropertyName("image_packaging_thumb_url")]
    public string? ImagePackagingThumbUrl { get; set; }

    [JsonPropertyName("images")]
    public Dictionary<string, object>? Images { get; set; }

    [JsonPropertyName("carbon_footprint_percent_of_known_ingredients")]
    public decimal? CarbonFootprintPercentOfKnownIngredients { get; set; }

    [JsonPropertyName("environment_impact_level")]
    public string? EnvironmentImpactLevel { get; set; }

    [JsonPropertyName("environment_impact_level_tags")]
    public List<string>? EnvironmentImpactLevelTags { get; set; }

    [JsonPropertyName("packaging_recycling_tags")]
    public List<string>? PackagingRecyclingTags { get; set; }

    [JsonPropertyName("packagings")]
    [JsonIgnore]
    public List<OffFullPackaging>? Packagings { get; set; }

    [JsonPropertyName("packaging_materials_tags")]
    public List<string>? PackagingMaterialsTags { get; set; }

    [JsonPropertyName("ecoscore_data")]
    public OffEcoscoreData? EcoscoreData { get; set; }
}

public class OffFullIngredient
{
    public string? Id { get; set; }
    public string? Text { get; set; }
    public decimal? Percent { get; set; }

    [JsonPropertyName("percent_min")]
    public decimal? PercentMin { get; set; }

    [JsonPropertyName("percent_max")]
    public decimal? PercentMax { get; set; }

    [JsonPropertyName("percent_estimate")]
    public decimal? PercentEstimate { get; set; }

    public string? Vegan { get; set; }
    public string? Vegetarian { get; set; }

    [JsonPropertyName("from_palm_oil")]
    public string? FromPalmOil { get; set; }

    public List<OffFullIngredient>? Ingredients { get; set; }
}

public class OffFullNutriments
{
    [JsonPropertyName("energy-kcal_100g")]
    public decimal? EnergyKcal100g { get; set; }

    [JsonPropertyName("energy-kj_100g")]
    public decimal? EnergyKj100g { get; set; }

    [JsonPropertyName("energy-kcal_serving")]
    public decimal? EnergyKcalServing { get; set; }

    [JsonPropertyName("energy-kj_serving")]
    public decimal? EnergyKjServing { get; set; }

    [JsonPropertyName("fat_100g")]
    public decimal? Fat100g { get; set; }

    [JsonPropertyName("fat_serving")]
    public decimal? FatServing { get; set; }

    [JsonPropertyName("saturated-fat_100g")]
    public decimal? SaturatedFat100g { get; set; }

    [JsonPropertyName("saturated-fat_serving")]
    public decimal? SaturatedFatServing { get; set; }

    [JsonPropertyName("monounsaturated-fat_100g")]
    public decimal? MonounsaturatedFat100g { get; set; }

    [JsonPropertyName("polyunsaturated-fat_100g")]
    public decimal? PolyunsaturatedFat100g { get; set; }

    [JsonPropertyName("trans-fat_100g")]
    public decimal? TransFat100g { get; set; }

    [JsonPropertyName("cholesterol_100g")]
    public decimal? Cholesterol100g { get; set; }

    [JsonPropertyName("omega-3-fat_100g")]
    public decimal? Omega3Fat100g { get; set; }

    [JsonPropertyName("omega-6-fat_100g")]
    public decimal? Omega6Fat100g { get; set; }

    [JsonPropertyName("carbohydrates_100g")]
    public decimal? Carbohydrates100g { get; set; }

    [JsonPropertyName("carbohydrates_serving")]
    public decimal? CarbohydratesServing { get; set; }

    [JsonPropertyName("sugars_100g")]
    public decimal? Sugars100g { get; set; }

    [JsonPropertyName("sugars_serving")]
    public decimal? SugarsServing { get; set; }

    [JsonPropertyName("starch_100g")]
    public decimal? Starch100g { get; set; }

    [JsonPropertyName("polyols_100g")]
    public decimal? Polyols100g { get; set; }

    [JsonPropertyName("fiber_100g")]
    public decimal? Fiber100g { get; set; }

    [JsonPropertyName("fiber_serving")]
    public decimal? FiberServing { get; set; }

    [JsonPropertyName("proteins_100g")]
    public decimal? Proteins100g { get; set; }

    [JsonPropertyName("proteins_serving")]
    public decimal? ProteinsServing { get; set; }

    [JsonPropertyName("salt_100g")]
    public decimal? Salt100g { get; set; }

    [JsonPropertyName("salt_serving")]
    public decimal? SaltServing { get; set; }

    [JsonPropertyName("sodium_100g")]
    public decimal? Sodium100g { get; set; }

    [JsonPropertyName("sodium_serving")]
    public decimal? SodiumServing { get; set; }

    [JsonPropertyName("vitamin-a_100g")]
    public decimal? VitaminA100g { get; set; }

    [JsonPropertyName("vitamin-b1_100g")]
    public decimal? VitaminB1100g { get; set; }

    [JsonPropertyName("vitamin-b2_100g")]
    public decimal? VitaminB2100g { get; set; }

    [JsonPropertyName("vitamin-b6_100g")]
    public decimal? VitaminB6100g { get; set; }

    [JsonPropertyName("vitamin-b9_100g")]
    public decimal? VitaminB9100g { get; set; }

    [JsonPropertyName("vitamin-b12_100g")]
    public decimal? VitaminB12100g { get; set; }

    [JsonPropertyName("vitamin-c_100g")]
    public decimal? VitaminC100g { get; set; }

    [JsonPropertyName("vitamin-d_100g")]
    public decimal? VitaminD100g { get; set; }

    [JsonPropertyName("vitamin-e_100g")]
    public decimal? VitaminE100g { get; set; }

    [JsonPropertyName("vitamin-k_100g")]
    public decimal? VitaminK100g { get; set; }

    [JsonPropertyName("vitamin-pp_100g")]
    public decimal? VitaminPp100g { get; set; }

    [JsonPropertyName("calcium_100g")]
    public decimal? Calcium100g { get; set; }

    [JsonPropertyName("iron_100g")]
    public decimal? Iron100g { get; set; }

    [JsonPropertyName("magnesium_100g")]
    public decimal? Magnesium100g { get; set; }

    [JsonPropertyName("zinc_100g")]
    public decimal? Zinc100g { get; set; }

    [JsonPropertyName("phosphorus_100g")]
    public decimal? Phosphorus100g { get; set; }

    [JsonPropertyName("potassium_100g")]
    public decimal? Potassium100g { get; set; }

    [JsonPropertyName("iodine_100g")]
    public decimal? Iodine100g { get; set; }

    [JsonPropertyName("selenium_100g")]
    public decimal? Selenium100g { get; set; }

    [JsonPropertyName("copper_100g")]
    public decimal? Copper100g { get; set; }

    [JsonPropertyName("manganese_100g")]
    public decimal? Manganese100g { get; set; }

    [JsonPropertyName("fluoride_100g")]
    public decimal? Fluoride100g { get; set; }

    [JsonPropertyName("caffeine_100g")]
    public decimal? Caffeine100g { get; set; }

    [JsonPropertyName("taurine_100g")]
    public decimal? Taurine100g { get; set; }

    [JsonPropertyName("alcohol_100g")]
    public decimal? Alcohol100g { get; set; }
}

public class OffNutrientLevels
{
    public string? Fat { get; set; }

    [JsonPropertyName("saturated-fat")]
    public string? SaturatedFat { get; set; }

    public string? Sugars { get; set; }
    public string? Salt { get; set; }
}

public class OffFullPackaging
{
    public string? Shape { get; set; }
    public string? Material { get; set; }
    public string? Recycling { get; set; }

    [JsonPropertyName("weight_measured")]
    public decimal? WeightMeasured { get; set; }

    [JsonPropertyName("quantity_per_unit")]
    public decimal? QuantityPerUnit { get; set; }
}

public class OffEcoscoreData
{
    public decimal? Score { get; set; }
    public string? Grade { get; set; }

    public OffAgribalyse? Agribalyse { get; set; }
    public OffEcoscoreAdjustments? Adjustments { get; set; }
}

public class OffAgribalyse
{
    [JsonPropertyName("agribalyse_food_code")]
    public string? AgribalyseFoodCode { get; set; }

    [JsonPropertyName("agribalyse_food_name_en")]
    public string? AgribalyseFoodNameEn { get; set; }

    [JsonPropertyName("co2_agriculture")]
    public decimal? Co2Agriculture { get; set; }

    [JsonPropertyName("co2_consumption")]
    public decimal? Co2Consumption { get; set; }

    [JsonPropertyName("co2_distribution")]
    public decimal? Co2Distribution { get; set; }

    [JsonPropertyName("co2_packaging")]
    public decimal? Co2Packaging { get; set; }

    [JsonPropertyName("co2_processing")]
    public decimal? Co2Processing { get; set; }

    [JsonPropertyName("co2_transportation")]
    public decimal? Co2Transportation { get; set; }

    [JsonPropertyName("co2_total")]
    public decimal? Co2Total { get; set; }

    [JsonPropertyName("ef_single_score")]
    public decimal? EfSingleScore { get; set; }
}

public class OffEcoscoreAdjustments
{
    public OffEcoscorePackaging? Packaging { get; set; }
    public OffEcoscoreOrigins? Origins { get; set; }

    [JsonPropertyName("production_system")]
    public OffEcoscoreProductionSystem? ProductionSystem { get; set; }

    [JsonPropertyName("threatened_species")]
    public OffEcoscoreThreatenedSpecies? ThreatenedSpecies { get; set; }
}

public class OffEcoscorePackaging
{
    public decimal? Value { get; set; }
    public string? Warning { get; set; }
}

public class OffEcoscoreOrigins
{
    [JsonPropertyName("epi_value")]
    public decimal? EpiValue { get; set; }

    [JsonPropertyName("transportation_value")]
    public decimal? TransportationValue { get; set; }
}

public class OffEcoscoreProductionSystem
{
    public decimal? Value { get; set; }
}

public class OffEcoscoreThreatenedSpecies
{
    public decimal? Value { get; set; }
}
