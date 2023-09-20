using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SvgIconViewer;

/// <summary>
/// Converter that takes a boolean as input and returns one of two properties: True or False
/// </summary>
public abstract class BooleanConverter<T> : IValueConverter
{
    public BooleanConverter(T trueValue, T falseValue)
    {
        this.True = trueValue;
        this.False = falseValue;
    }

    public T True { get; set; }
    public T False { get; set; }

    public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            bool boolean => boolean ? this.True : this.False,
            _ => DependencyProperty.UnsetValue
        };
    }

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
}
