﻿namespace DalApi;
using System.Xml.Linq;

static class DalConfig
{
    internal static string? s_dalName;
    internal static Dictionary<string, string> s_dalPackages;
    internal static Dictionary<string, string> s_dalNamespaces;
    internal static Dictionary<string, string> s_class;

    static DalConfig()
    {
        XElement dalConfig = XElement.Load(@"..\xml\dal-config.xml")
            ?? throw new Do.DalConfigException("dal-config.xml file is not found");
        s_dalName = dalConfig?.Element("dal")?.Value
            ?? throw new Do.DalConfigException("<dal> element is missing");
        var packages = dalConfig?.Element("dal-packages")?.Elements()
            ?? throw new Do.DalConfigException("<dal-packages> element is missing");
        s_dalPackages = packages.ToDictionary(p => "" + p.Name, p => p.Value);
        s_dalNamespaces = packages.ToDictionary(p => "" + p.Name, p => p.Attributes().FirstOrDefault(x => x.Name == "namespace")!.Value);
        s_class= packages.ToDictionary(p => "" + p.Name, p => p.Attributes().FirstOrDefault(x => x.Name == "class")!.Value);
    }
}
