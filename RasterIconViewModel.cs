using Humanizer;
using System;

namespace IconCatalog;

public class RasterIconViewModel : IIconViewModel
{
    public RasterIconViewModel(string fullPath)
    {
        this.Path = fullPath;
        this.Name = System.IO.Path.GetFileNameWithoutExtension(fullPath)
            .Replace("icons", "", StringComparison.InvariantCultureIgnoreCase)
            .Replace("icon", "", StringComparison.InvariantCultureIgnoreCase)
            .Humanize();
        this.Collection = System.IO.Path.GetDirectoryName(fullPath);
    }

    public string Path { get; set; }
    public string? Collection { get; set; }
    public string Name { get; set; }
}
