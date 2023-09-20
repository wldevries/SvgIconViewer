using System.Windows.Input;

namespace IconCatalog;

public class MouseWheelGesture : MouseGesture
{
    public MouseWheelGesture() : base(MouseAction.WheelClick)
    {
    }

    public MouseWheelGesture(ModifierKeys modifiers) : base(MouseAction.WheelClick, modifiers)
    {
    }

    public static MouseWheelGesture CtrlAltDown => new(ModifierKeys.Control | ModifierKeys.Alt)
    {
        Direction = WheelDirection.Down
    };

    public static MouseWheelGesture CtrlDown => new(ModifierKeys.Control)
    {
        Direction = WheelDirection.Down
    };

    public static MouseWheelGesture CtrlUp => new(ModifierKeys.Control)
    {
        Direction = WheelDirection.Up
    };

    public static MouseWheelGesture CtrlAltUp => new(ModifierKeys.Control | ModifierKeys.Alt)
    {
        Direction = WheelDirection.Up
    };

    public WheelDirection Direction { get; set; }

    public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
    {
        return base.Matches(targetElement, inputEventArgs)
            && inputEventArgs is MouseWheelEventArgs args
            && Direction switch
            {
                WheelDirection.None => args.Delta == 0,
                WheelDirection.Up => args.Delta > 0,
                WheelDirection.Down => args.Delta < 0,
                _ => false,
            };
    }
}

public enum WheelDirection
{
    None,
    Up,
    Down,
}
