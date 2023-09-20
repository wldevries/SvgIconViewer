using System.Windows;

namespace IconCatalog;

/// <summary>
/// Configurable converter that converts a boolean to <see cref="Visibility"/>.
/// </summary>
public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
{
    public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
    {
    }
}