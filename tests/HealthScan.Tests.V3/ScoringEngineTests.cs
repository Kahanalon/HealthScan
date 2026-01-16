using FluentAssertions;
using HealthScan.Application.Scoring;
using HealthScan.Domain.Entities;
using HealthScan.Domain.Enums;
using Xunit;

namespace HealthScan.Tests.V3;

public class ScoringEngineTests
{
    private readonly CustomScoringEngine _scoringEngine;
    private readonly RegexIngredientAnalyzer _ingredientAnalyzer;

    public ScoringEngineTests()
    {
        _ingredientAnalyzer = new RegexIngredientAnalyzer();
        _scoringEngine = new CustomScoringEngine(_ingredientAnalyzer);
    }

    [Fact]
    public void CanScore_WithCompleteNutrition_ReturnsTrue()
    {
        var product = CreateProduct(sugars: 10, sodium: 200, saturatedFat: 3);

        var result = _scoringEngine.CanScore(product);

        result.Should().BeTrue();
    }

    [Fact]
    public void CanScore_WithMissingSugars_ReturnsFalse()
    {
        var product = CreateProduct(sugars: null, sodium: 200, saturatedFat: 3);

        var result = _scoringEngine.CanScore(product);

        result.Should().BeFalse();
    }

    [Fact]
    public void CanScore_WithMissingSodium_ReturnsFalse()
    {
        var product = CreateProduct(sugars: 10, sodium: null, saturatedFat: 3);

        var result = _scoringEngine.CanScore(product);

        result.Should().BeFalse();
    }

    [Fact]
    public void CanScore_WithMissingSaturatedFat_ReturnsFalse()
    {
        var product = CreateProduct(sugars: 10, sodium: 200, saturatedFat: null);

        var result = _scoringEngine.CanScore(product);

        result.Should().BeFalse();
    }

    [Fact]
    public void CalculateScore_WithIncompleteData_ReturnsIncompleteResult()
    {
        var product = CreateProduct(sugars: null, sodium: 200, saturatedFat: 3);

        var result = _scoringEngine.CalculateScore(product);

        result.IsComplete.Should().BeFalse();
        result.Value.Should().BeNull();
        result.MissingFields.Should().Contain("sugars_100g");
    }

    [Fact]
    public void CalculateScore_WithHealthyProduct_ReturnsHighScore()
    {
        var product = CreateProduct(
            sugars: 3,
            sodium: 50,
            saturatedFat: 1,
            fiber: 6,
            protein: 12);

        var result = _scoringEngine.CalculateScore(product);

        result.IsComplete.Should().BeTrue();
        result.Value.Should().BeGreaterOrEqualTo(80);
        result.Grade.Should().Be(Grade.A);
    }

    [Fact]
    public void CalculateScore_WithHighSugar_AppliesPenalty()
    {
        var product = CreateProduct(sugars: 20, sodium: 50, saturatedFat: 1);

        var result = _scoringEngine.CalculateScore(product);

        result.IsComplete.Should().BeTrue();
        result.Value.Should().BeLessThan(100);
        result.Flags.Should().Contain(f => f.Type == FlagType.HighSugar);
    }

    [Fact]
    public void CalculateScore_WithHighSodium_AppliesPenalty()
    {
        var product = CreateProduct(sugars: 5, sodium: 600, saturatedFat: 1);

        var result = _scoringEngine.CalculateScore(product);

        result.IsComplete.Should().BeTrue();
        result.Value.Should().BeLessThan(100);
        result.Flags.Should().Contain(f => f.Type == FlagType.HighSodium);
    }

    [Fact]
    public void CalculateScore_WithHighSaturatedFat_AppliesPenalty()
    {
        var product = CreateProduct(sugars: 5, sodium: 50, saturatedFat: 8);

        var result = _scoringEngine.CalculateScore(product);

        result.IsComplete.Should().BeTrue();
        result.Value.Should().BeLessThan(100);
        result.Flags.Should().Contain(f => f.Type == FlagType.HighSaturatedFat);
    }

    [Fact]
    public void CalculateScore_WithAllBadNutrients_AppliesAllPenalties()
    {
        var product = CreateProduct(
            sugars: 25,
            sodium: 700,
            saturatedFat: 10,
            fiber: 0);

        var result = _scoringEngine.CalculateScore(product);

        result.IsComplete.Should().BeTrue();
        result.Value.Should().BeLessThanOrEqualTo(40);
        result.Grade.Should().BeOneOf(Grade.D, Grade.E);
    }

    [Fact]
    public void CalculateScore_WithHighFiber_AppliesBonus()
    {
        var product = CreateProduct(sugars: 5, sodium: 50, saturatedFat: 1, fiber: 8);

        var result = _scoringEngine.CalculateScore(product);

        result.TopReasons.Should().Contain(r => r.Contains("fiber"));
    }

    [Fact]
    public void CalculateScore_WithHighProtein_AppliesBonus()
    {
        var product = CreateProduct(sugars: 5, sodium: 50, saturatedFat: 1, protein: 15);

        var result = _scoringEngine.CalculateScore(product);

        result.TopReasons.Should().Contain(r => r.Contains("protein"));
    }

    [Fact]
    public void CalculateScore_WithArtificialSweeteners_AppliesPenalty()
    {
        var product = CreateProduct(sugars: 0, sodium: 50, saturatedFat: 1);
        product.IngredientsTextEn = "water, aspartame, citric acid";

        var result = _scoringEngine.CalculateScore(product);

        result.Flags.Should().Contain(f => f.Type == FlagType.ArtificialSweetener);
    }

    [Fact]
    public void CalculateScore_WithPalmOil_AppliesPenalty()
    {
        var product = CreateProduct(sugars: 5, sodium: 50, saturatedFat: 3);
        product.IngredientsTextEn = "wheat flour, palm oil, sugar, salt";

        var result = _scoringEngine.CalculateScore(product);

        result.Flags.Should().Contain(f => f.Type == FlagType.PalmOil);
    }

    [Fact]
    public void CalculateScore_WithHebrewIngredients_DetectsFlags()
    {
        var product = CreateProduct(sugars: 5, sodium: 50, saturatedFat: 3);
        product.IngredientsTextHe = "קמח חיטה, שמן דקלים, סוכר, מלח";

        var result = _scoringEngine.CalculateScore(product);

        result.Flags.Should().Contain(f => f.Type == FlagType.PalmOil);
    }

    [Fact]
    public void CalculateScore_ScoreIsClamped_Between0And100()
    {
        var veryBadProduct = CreateProduct(
            sugars: 50,
            sodium: 2000,
            saturatedFat: 30,
            fiber: 0);
        veryBadProduct.IngredientsTextEn = "sugar, palm oil, aspartame, MSG, E110, sodium benzoate";

        var result = _scoringEngine.CalculateScore(veryBadProduct);

        result.Value.Should().BeGreaterOrEqualTo(0);
        result.Value.Should().BeLessThanOrEqualTo(100);
    }

    [Fact]
    public void CalculateScore_GradeA_ForScoreAbove80()
    {
        var product = CreateProduct(sugars: 2, sodium: 30, saturatedFat: 1, fiber: 6, protein: 12);

        var result = _scoringEngine.CalculateScore(product);

        result.Grade.Should().Be(Grade.A);
        result.Value.Should().BeGreaterOrEqualTo(80);
    }

    [Fact]
    public void CalculateScore_LowGrade_ForVeryBadProduct()
    {
        var product = CreateProduct(sugars: 40, sodium: 900, saturatedFat: 15, fiber: 0, protein: 1);

        var result = _scoringEngine.CalculateScore(product);

        result.Grade.Should().BeOneOf(Grade.D, Grade.E);
        result.Value.Should().BeLessThan(40);
    }

    private static Product CreateProduct(
        decimal? sugars = 5,
        decimal? sodium = 100,
        decimal? saturatedFat = 2,
        decimal? fiber = 2,
        decimal? protein = 5)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Barcode = "1234567890",
            NameEn = "Test Product",
            Sugars100g = sugars,
            Sodium100g = sodium,
            SaturatedFat100g = saturatedFat,
            Fiber100g = fiber,
            Protein100g = protein
        };
    }
}
