using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IconCatalog;

/// <summary>
/// Icon control based on the opacity layer of an image. The icon fill color is determined by the Foreground property.
/// </summary>
[TemplatePart(Name = "PART_IconRectangle", Type = typeof(Rectangle))]
public class ImageIcon : Control
{
    public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(ImageIcon), new PropertyMetadata(null));

    private Rectangle? iconRectangle;

    static ImageIcon()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageIcon), new FrameworkPropertyMetadata(typeof(ImageIcon)));

        ForegroundProperty.OverrideMetadata(typeof(ImageIcon), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), HandleForegroundChanged));
    }

    public ImageSource Image
    {
        get { return (ImageSource)GetValue(ImageProperty); }
        set { SetValue(ImageProperty, value); }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this.iconRectangle = this.Template.FindName("PART_IconRectangle", this) as Rectangle;

        this.UpdateState();
    }

    private static void HandleForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        ((ImageIcon)d).UpdateState();
    }

    private void UpdateState()
    {
        if (this.iconRectangle != null)
        {
            this.iconRectangle.Fill = this.Foreground;
        }
    }
}