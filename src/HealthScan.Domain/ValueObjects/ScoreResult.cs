using HealthScan.Domain.Enums;

namespace HealthScan.Domain.ValueObjects;

public record ScoreResult
{
    public int? Value { get; init; }
    public Grade? Grade { get; init; }
    public string? Color { get; init; }
    public bool IsComplete { get; init; }
    public List<string> MissingFields { get; init; } = new();
    public List<ScoreFlag> Flags { get; init; } = new();
    public List<string> TopReasons { get; init; } = new();

    public static ScoreResult Incomplete(List<string> missingFields) => new()
    {
        Value = null,
        Grade = null,
        Color = null,
        IsComplete = false,
        MissingFields = missingFields
    };

    public static ScoreResult FromScore(int score, List<ScoreFlag> flags, List<string> reasons)
    {
        var grade = GetGrade(score);
        return new ScoreResult
        {
            Value = score,
            Grade = grade,
            Color = GetColor(grade),
            IsComplete = true,
            Flags = flags,
            TopReasons = reasons.Take(3).ToList()
        };
    }

    private static Grade GetGrade(int score) => score switch
    {
        >= 80 => Enums.Grade.A,
        >= 60 => Enums.Grade.B,
        >= 40 => Enums.Grade.C,
        >= 20 => Enums.Grade.D,
        _ => Enums.Grade.E
    };

    private static string GetColor(Grade grade) => grade switch
    {
        Enums.Grade.A => "#22C55E",
        Enums.Grade.B => "#84CC16",
        Enums.Grade.C => "#FFA500",
        Enums.Grade.D => "#F97316",
        Enums.Grade.E => "#EF4444",
        _ => "#808080"
    };
}

public record ScoreFlag
{
    public FlagType Type { get; init; }
    public string Description { get; init; } = string.Empty;
    public int PenaltyPoints { get; init; }
}
