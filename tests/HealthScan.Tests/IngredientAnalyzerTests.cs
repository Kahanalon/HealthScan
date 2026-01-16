using FluentAssertions;
using HealthScan.Application.Scoring;
using HealthScan.Domain.Enums;
using Xunit;

namespace HealthScan.Tests;

public class IngredientAnalyzerTests
{
    private readonly RegexIngredientAnalyzer _analyzer;

    public IngredientAnalyzerTests()
    {
        _analyzer = new RegexIngredientAnalyzer();
    }

    [Fact]
    public void ParseIngredients_SplitsByComma()
    {
        var text = "sugar, salt, flour, water";

        var result = _analyzer.ParseIngredients(text);

        result.Should().HaveCount(4);
        result.Should().Contain("sugar");
    }

    [Fact]
    public void ParseIngredients_SplitsBySemicolon()
    {
        var text = "sugar; salt; flour";

        var result = _analyzer.ParseIngredients(text);

        result.Should().HaveCount(3);
    }

    [Fact]
    public void ParseIngredients_HandlesEmptyString()
    {
        var result = _analyzer.ParseIngredients("");

        result.Should().BeEmpty();
    }

    [Fact]
    public void AnalyzeIngredients_DetectsAspartame()
    {
        var text = "water, aspartame, citric acid";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.ArtificialSweetener);
    }

    [Fact]
    public void AnalyzeIngredients_DetectsSucralose()
    {
        var text = "water, sucralose, flavor";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.ArtificialSweetener);
    }

    [Fact]
    public void AnalyzeIngredients_DetectsPalmOil()
    {
        var text = "wheat flour, palm oil, sugar";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.PalmOil);
    }

    [Fact]
    public void AnalyzeIngredients_DetectsMSG()
    {
        var text = "salt, MSG, spices";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.FlavorEnhancer);
    }

    [Fact]
    public void AnalyzeIngredients_DetectsHebrewAspartame()
    {
        var text = "מים, אספרטיים, חומצת לימון";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.ArtificialSweetener);
    }

    [Fact]
    public void AnalyzeIngredients_DetectsHebrewPalmOil()
    {
        var text = "קמח חיטה, שמן דקלים, סוכר";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.PalmOil);
    }

    [Fact]
    public void AnalyzeIngredients_DetectsENumbers()
    {
        var text = "water, E110, citric acid";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.ArtificialColorant);
    }

    [Fact]
    public void AnalyzeIngredients_DetectsPreservatives()
    {
        var text = "water, sodium benzoate, citric acid";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().Contain(f => f.Type == FlagType.Preservative);
    }

    [Fact]
    public void AnalyzeIngredients_DoesNotDuplicateFlags()
    {
        var text = "aspartame, sucralose, acesulfame";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Count(f => f.Type == FlagType.ArtificialSweetener).Should().Be(1);
    }

    [Fact]
    public void AnalyzeIngredients_ReturnsEmptyForCleanIngredients()
    {
        var text = "milk, water, salt";

        var flags = _analyzer.AnalyzeIngredients(text);

        flags.Should().BeEmpty();
    }
}
