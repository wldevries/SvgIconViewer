using System.Windows.Input;

namespace IconCatalog;

public static class AppCommands
{
    public static RoutedCommand DarkModeToggle { get; } = new(nameof(DarkModeToggle), typeof(AppCommands));
    public static RoutedCommand OutlineToggle { get; } = new(nameof(OutlineToggle), typeof(AppCommands));
    public static RoutedCommand ColorToggle { get; } = new(nameof(DarkModeToggle), typeof(AppCommands));
}
