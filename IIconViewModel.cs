namespace SvgIconViewer
{
    public interface IIconViewModel
    {
        string? Collection { get; set; }
        string Name { get; set; }
        string Path { get; set; }
    }
}