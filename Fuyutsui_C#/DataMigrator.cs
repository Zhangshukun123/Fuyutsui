namespace FuyutsuiCSharp;

public static class DataMigrator
{
    public static void ExportYamlToJson()
    {
        var source = AppPaths.FindPythonDataDirectory();
        var target = AppPaths.FindCSharpDataDirectory(create: true);
        ExportConfig(source, target);
        ExportKeymaps(source, target);
    }

    private static void ExportConfig(DirectoryInfo source, DirectoryInfo target)
    {
        var config = MiniYaml.LoadFile(Path.Combine(source.FullName, "config.yml"));
        RewriteKeymapExtensions(config);
        JsonData.SaveObject(Path.Combine(target.FullName, "config.json"), config);
    }

    private static void RewriteKeymapExtensions(Dictionary<string, object?> config)
    {
        foreach (var value in config.Values)
        {
            if (MiniYaml.AsMap(value) is not { } classConfig)
            {
                continue;
            }

            var keymap = MiniYaml.AsString(classConfig.GetValueOrDefault("keymap"));
            if (string.IsNullOrWhiteSpace(keymap))
            {
                continue;
            }

            classConfig["keymap"] = Path.ChangeExtension(keymap, ".json");
        }
    }

    private static void ExportKeymaps(DirectoryInfo source, DirectoryInfo target)
    {
        var sourceDir = Path.Combine(source.FullName, "keymap");
        var targetDir = Path.Combine(target.FullName, "keymaps");
        Directory.CreateDirectory(targetDir);

        foreach (var sourcePath in Directory.EnumerateFiles(sourceDir, "*.yml"))
        {
            var root = MiniYaml.LoadFile(sourcePath);
            var entries = root
                .Select(kv => (Id: MiniYaml.AsInt(kv.Key) ?? 0, Entry: MiniYaml.AsMap(kv.Value)))
                .Where(item => item.Entry is not null)
                .OrderBy(item => item.Id)
                .Select(item => NormalizeKeymapEntry(item.Id, item.Entry!))
                .ToList();

            var targetPath = Path.Combine(targetDir, Path.ChangeExtension(Path.GetFileName(sourcePath), ".json"));
            JsonData.SaveObject(targetPath, entries);
        }
    }

    private static Dictionary<string, object?> NormalizeKeymapEntry(int id, Dictionary<string, object?> entry)
    {
        var result = new Dictionary<string, object?>(StringComparer.Ordinal)
        {
            ["id"] = id,
            ["unit"] = MiniYaml.AsInt(entry.GetValueOrDefault("unit")) ?? 0,
            ["spell"] = MiniYaml.AsString(entry.GetValueOrDefault("spell"))
                ?? MiniYaml.AsString(entry.GetValueOrDefault("技能"))
                ?? "",
            ["hotkey"] = MiniYaml.AsString(entry.GetValueOrDefault("hotkey"))
                ?? MiniYaml.AsString(entry.GetValueOrDefault("热键"))
                ?? "",
        };

        return result;
    }
}
