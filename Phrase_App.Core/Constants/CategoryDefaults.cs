namespace Phrase_App.Core.Constants;

public static class CategoryDefaults
{
    public static readonly Dictionary<string, string> Icons = new()
    {
        { "growth", "growth" }, { "focus", "focus" }, { "zen", "zen" },
        { "wisdom", "wisdom" }, { "energy", "energy" }, { "career", "career" },
        { "wealth", "wealth" }, { "discipline", "discipline" }, { "love", "love" },
        { "peace", "peace" }, { "gratitude", "gratitude" }, { "happiness", "happiness" },
        { "resilience", "resilience" }, { "courage", "courage" }, { "fitness", "fitness" },
        { "hope", "hope" }
    };

    public static readonly List<string> Colors = new()
    {
        "#00E676", "#FF5252", "#40C4FF", "#7C4DFF", "#FFD740", "#FF9100",
        "#00C853", "#607D8B", "#FF4081", "#00B8D4", "#FF80AB", "#FBC02D",
        "#A1887F", "#D50000", "#2962FF", "#B2FF59"
    };
}