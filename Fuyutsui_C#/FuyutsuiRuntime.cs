namespace FuyutsuiCSharp;

public sealed class FuyutsuiRuntime
{
    public FuyutsuiRuntime()
    {
        DataDirectory = ResolveDataDirectory();
        var configPath = Path.Combine(DataDirectory.FullName, "config.json");
        if (!File.Exists(configPath))
        {
            configPath = Path.Combine(DataDirectory.FullName, "config.yml");
        }

        Scanner = new PixelScanner();
        Decoder = new ConfigDecoder(configPath);
        Keymaps = new KeymapService(DataDirectory, Decoder.RawConfig);
    }

    public DirectoryInfo DataDirectory { get; }
    public PixelScanner Scanner { get; }
    public ConfigDecoder Decoder { get; }
    public KeymapService Keymaps { get; }

    public Dictionary<string, object?>? GetInfo(string windowTitle = "魔兽世界")
    {
        var scan = Scanner.ScanScreenData(windowTitle);
        var info = Decoder.BuildInfo(scan.RowData, scan.BarData);
        var classId = info.GetInt("职业");
        if (classId is not null)
        {
            Keymaps.SelectKeymapForClass(classId);
        }

        return info;
    }

    private static DirectoryInfo ResolveDataDirectory()
    {
        var csharpData = AppPaths.FindCSharpDataDirectory();
        if (File.Exists(Path.Combine(csharpData.FullName, "config.json")))
        {
            return csharpData;
        }

        return AppPaths.FindPythonDataDirectory();
    }
}

public static class StateDictionaryExtensions
{
    public static int? GetInt(this Dictionary<string, object?>? state, string key)
    {
        if (state is null || !state.TryGetValue(key, out var value))
        {
            return null;
        }

        return value switch
        {
            int i => i,
            bool b => b ? 1 : 0,
            string s when int.TryParse(s, out var i) => i,
            _ => null,
        };
    }

    public static bool GetBool(this Dictionary<string, object?>? state, string key)
    {
        if (state is null || !state.TryGetValue(key, out var value))
        {
            return false;
        }

        return value switch
        {
            bool b => b,
            int i => i != 0,
            string s when int.TryParse(s, out var i) => i != 0,
            _ => false,
        };
    }
}
