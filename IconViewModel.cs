﻿using Humanizer;

namespace SvgIconViewer;

public class IconViewModel
{
    public IconViewModel(string fullPath)
    {
        this.Path = fullPath;
        this.Name = System.IO.Path.GetFileNameWithoutExtension(fullPath).Humanize();
        this.Collection = System.IO.Path.GetDirectoryName(fullPath);
    }

    public string Path { get; set; }
    public string? Collection { get; set; }
    public string Name { get; set; }
}
