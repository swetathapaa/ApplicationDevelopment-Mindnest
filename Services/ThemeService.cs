using Microsoft.AspNetCore.Components;

public class ThemeService
{
    // Existing colors for backward compatibility
    public string BackgroundColor { get; private set; } = "#fff8db";
    public string TextColor { get; private set; } = "#867766";

    // New colors for TodayJournal (cards, buttons, pills, inputs, secondary text)
    public string SecondaryTextColor { get; private set; } = "#7A7A7A";
    public string CardBackground { get; private set; } = "#F8F6F3";
    public string ButtonBackground { get; private set; } = "#c5996e";
    public string ButtonText { get; private set; } = "#FFFFFF";
    public string PillBackground { get; private set; } = "#efede9";
    public string PillActiveBackground { get; private set; } = "#d9d4cc";
    public string InputBackground { get; private set; } = "#F8F6F3";

    public event Action? OnThemeChanged;

    public void SetTheme(bool isDark)
    {
        if (isDark)
        {
            BackgroundColor = "#1e1e1e";
            TextColor = "#ffffff";

            // Update additional colors for dark mode
            SecondaryTextColor = "#D3D3D3";
            CardBackground = "#3B3B3B";
            ButtonBackground = "#5B9A8B";
            ButtonText = "#FFFFFF";
            PillBackground = "#5B9A8B";
            PillActiveBackground = "#E6C79C";
            InputBackground = "#3B3B3B";
        }
        else
        {
            BackgroundColor = "#fff8db";
            TextColor = "#867766";

            // Update additional colors for light mode
            SecondaryTextColor = "#7A7A7A";
            CardBackground = "#F8F6F3";
            ButtonBackground = "#c5996e";
            ButtonText = "#FFFFFF";
            PillBackground = "#efede9";
            PillActiveBackground = "#d9d4cc";
            InputBackground = "#F8F6F3";
        }

        OnThemeChanged?.Invoke();
    }
}