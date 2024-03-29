﻿namespace SharpVPK;

public class VpkDirectory
{
    public List<VpkEntry> Entries { get; set; }
    public string Path { get; set; }
    internal VpkArchive ParentArchive { get; set; }

    internal VpkDirectory(VpkArchive parentArchive, string path, List<VpkEntry> entries)
    {
        ParentArchive = parentArchive;
        Path = path;
        Entries = entries;
    }
}
