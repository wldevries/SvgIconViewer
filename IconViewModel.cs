using Humanizer;
using System;

namespace IconCatalog;

public class IconViewModel
{
    public IconViewModel(string fullPath)
    {
        this.Path = fullPath;
        this.FileName = System.IO.Path.GetFileName(this.Path);
        this.Name = System.IO.Path.GetFileNameWithoutExtension(fullPath)
            .Replace("icons", "", StringComparison.InvariantCultureIgnoreCase)
            .Replace("icon", "", StringComparison.InvariantCultureIgnoreCase)
            .Humanize();
        this.Collection = System.IO.Path.GetDirectoryName(fullPath);
    }

    public string? Collection { get; }
    public string Name { get; }
    public string FileName { get; } 
    public string Path { get; }
}