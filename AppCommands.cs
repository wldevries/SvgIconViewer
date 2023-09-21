using System.Windows.Input;

namespace IconCatalog;

public static class AppCommands
{
    public static RoutedCommand ToggleDarkMode { get; } = new(nameof(ToggleDarkMode), typeof(AppCommands));
    public static RoutedCommand ToggleOutline { get; } = new(nameof(ToggleOutline), typeof(AppCommands));
    public static RoutedCommand ToggleColor { get; } = new(nameof(ToggleColor), typeof(AppCommands));
    public static RoutedCommand ToggleHumanizer { get; } = new(nameof(ToggleHumanizer), typeof(AppCommands));
}
