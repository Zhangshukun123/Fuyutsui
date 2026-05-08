namespace FuyutsuiCSharp;

public sealed class KeymapService
{
    private readonly DirectoryInfo _dataDirectory;
    private readonly IReadOnlyDictionary<string, object?> _config;
    private readonly Dictionary<(int Unit, string Spell), string> _hotkeys = new();
    private int? _currentClassId;
    private string? _currentPath;

    public KeymapService(DirectoryInfo dataDirectory, IReadOnlyDictionary<string, object?> config)
    {
        _dataDirectory = dataDirectory;
        _config = config;
    }

    public void SelectKeymapForClass(int? classId)
    {
        if (_hotkeys.Count > 0 && _currentClassId == classId)
        {
            return;
        }

        var keymapDirName = Directory.Exists(Path.Combine(_dataDirectory.FullName, "keymaps")) ? "keymaps" : "keymap";
        var defaultJson = Path.Combine(_dataDirectory.FullName, keymapDirName, "keymap.json");
        var defaultYml = Path.Combine(_dataDirectory.FullName, keymapDirName, "keymap.yml");
        var path = File.Exists(defaultJson) ? defaultJson : defaultYml;

        if (classId is not null &&
            MiniYaml.AsMap(_config.GetValueOrDefault(classId.Value.ToString())) is { } classConfig &&
            MiniYaml.AsString(classConfig.GetValueOrDefault("keymap")) is { Length: > 0 } keymapName)
        {
            path = Path.IsPathRooted(keymapName)
                ? keymapName
                : Path.Combine(_dataDirectory.FullName, keymapDirName, keymapName);
        }

        _currentClassId = classId;
        _currentPath = path;
        LoadKeymap(path);
    }

    public string? GetHotkey(int? unit, string spell)
    {
        if (_hotkeys.Count == 0)
        {
            SelectKeymapForClass(_currentClassId);
        }

        return _hotkeys.TryGetValue((unit ?? 0, spell), out var hotkey) ? hotkey : null;
    }

    public string? CurrentPath => _currentPath;

    private void LoadKeymap(string path)
    {
        _hotkeys.Clear();
        if (!File.Exists(path))
        {
            return;
        }

        if (string.Equals(Path.GetExtension(path), ".json", StringComparison.OrdinalIgnoreCase))
        {
            LoadJsonKeymap(path);
            return;
        }

        LoadYamlKeymap(path);
    }

    private void LoadYamlKeymap(string path)
    {
        var root = MiniYaml.LoadFile(path);
        foreach (var value in root.Values)
        {
            if (MiniYaml.AsMap(value) is not { } entry)
            {
                continue;
            }

            var unit = MiniYaml.AsInt(entry.GetValueOrDefault("unit")) ?? 0;
            var spell = MiniYaml.AsString(entry.GetValueOrDefault("spell"))
                ?? MiniYaml.AsString(entry.GetValueOrDefault("技能"));
            var hotkey = MiniYaml.AsString(entry.GetValueOrDefault("hotkey"))
                ?? MiniYaml.AsString(entry.GetValueOrDefault("热键"));

            if (!string.IsNullOrWhiteSpace(spell) && !string.IsNullOrWhiteSpace(hotkey))
            {
                _hotkeys[(unit, spell)] = hotkey;
            }
        }
    }

    private void LoadJsonKeymap(string path)
    {
        var root = JsonData.LoadObject(path);
        var entries = root switch
        {
            List<object?> list => list.OfType<Dictionary<string, object?>>(),
            Dictionary<string, object?> map => map.Values.OfType<Dictionary<string, object?>>(),
            _ => Enumerable.Empty<Dictionary<string, object?>>(),
        };

        foreach (var entry in entries)
        {
            var unit = MiniYaml.AsInt(entry.GetValueOrDefault("unit")) ?? 0;
            var spell = MiniYaml.AsString(entry.GetValueOrDefault("spell"))
                ?? MiniYaml.AsString(entry.GetValueOrDefault("技能"));
            var hotkey = MiniYaml.AsString(entry.GetValueOrDefault("hotkey"))
                ?? MiniYaml.AsString(entry.GetValueOrDefault("热键"));

            if (!string.IsNullOrWhiteSpace(spell) && !string.IsNullOrWhiteSpace(hotkey))
            {
                _hotkeys[(unit, spell)] = hotkey;
            }
        }
    }
}
