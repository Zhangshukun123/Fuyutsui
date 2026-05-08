namespace FuyutsuiCSharp;

public sealed class ConfigDecoder
{
    private static readonly string[] MetaPixelKeys = ["锚点", "职业", "专精"];
    private readonly Dictionary<string, object?> _config;

    public ConfigDecoder(string configPath)
    {
        _config = string.Equals(Path.GetExtension(configPath), ".json", StringComparison.OrdinalIgnoreCase)
            ? JsonData.LoadDictionary(configPath)
            : MiniYaml.LoadFile(configPath);
    }

    public IReadOnlyDictionary<string, object?> RawConfig => _config;

    public Dictionary<string, object?>? BuildInfo(Dictionary<int, int>? rowData, Dictionary<int, int>? barData)
    {
        if (rowData is null || rowData.Count == 0)
        {
            return null;
        }

        var classId = rowData.GetValueOrDefault(2);
        var specId = rowData.GetValueOrDefault(3);
        var stateConfig = GetSpecConfig(classId, specId);
        return BuildStateDictionary(rowData, barData ?? new Dictionary<int, int>(), stateConfig);
    }

    private Dictionary<string, object?> GetSpecConfig(int classId, int specId)
    {
        var merged = new Dictionary<string, object?>(StringComparer.Ordinal);

        foreach (var key in MetaPixelKeys)
        {
            if (MiniYaml.AsMap(_config.GetValueOrDefault(key)) is { } block && block.ContainsKey("step"))
            {
                merged[key] = block;
            }
        }

        if (MiniYaml.AsMap(_config.GetValueOrDefault("state")) is { } state)
        {
            foreach (var (key, value) in state)
            {
                merged[key] = value;
            }
        }

        var classMap = MiniYaml.AsMap(_config.GetValueOrDefault(classId.ToString()));
        var specMap = classMap is null ? null : MiniYaml.AsMap(classMap.GetValueOrDefault(specId.ToString()));
        if (specMap is not null)
        {
            foreach (var (key, value) in specMap)
            {
                merged[key] = value;
            }
        }

        return merged;
    }

    private static Dictionary<string, object?> BuildStateDictionary(
        Dictionary<int, int> rowData,
        Dictionary<int, int> barData,
        Dictionary<string, object?> stateConfig)
    {
        var result = new Dictionary<string, object?>(StringComparer.Ordinal);

        foreach (var (key, value) in stateConfig)
        {
            if (key is "group" or "spells")
            {
                continue;
            }

            if (FieldConfig.TryCreate(value, out var field))
            {
                result[key] = ConvertRaw(ResolveRaw(field, rowData, barData), field.Type);
            }
        }

        if (MiniYaml.AsMap(stateConfig.GetValueOrDefault("spells")) is { } spellsConfig)
        {
            var spells = new Dictionary<string, object?>(StringComparer.Ordinal);
            foreach (var (spellName, value) in spellsConfig)
            {
                if (FieldConfig.TryCreate(value, out var field))
                {
                    spells[spellName] = ConvertRaw(ResolveRaw(field, rowData, barData), field.Type);
                }
            }

            result["spells"] = spells;
        }

        if (MiniYaml.AsMap(stateConfig.GetValueOrDefault("group")) is { } groupConfig)
        {
            var start = MiniYaml.AsInt(groupConfig.GetValueOrDefault("start")) ?? 26;
            var num = MiniYaml.AsInt(groupConfig.GetValueOrDefault("num")) ?? 5;
            var group = new Dictionary<string, object?>(StringComparer.Ordinal);

            for (var unit = 1; unit <= 30; unit++)
            {
                var baseStep = start + (unit - 1) * num;
                var sub = new Dictionary<string, object?>(StringComparer.Ordinal);
                foreach (var (fieldName, value) in groupConfig)
                {
                    if (fieldName is "start" or "num")
                    {
                        continue;
                    }

                    if (!FieldConfig.TryCreate(value, out var field))
                    {
                        continue;
                    }

                    int? raw;
                    if (field.IsBar)
                    {
                        raw = ResolveRaw(field, rowData, barData);
                    }
                    else if (field.StepNumber is { } relStep)
                    {
                        raw = rowData.GetValueOrDefault(baseStep + relStep);
                    }
                    else
                    {
                        raw = null;
                    }

                    sub[fieldName] = ConvertRaw(raw, field.Type);
                }

                group[unit.ToString()] = sub;
            }

            result["group"] = group;
        }

        return result;
    }

    private static int? ResolveRaw(FieldConfig field, Dictionary<int, int> rowData, Dictionary<int, int> barData)
    {
        if (field.IsBar)
        {
            return field.BarNumber is { } bar ? barData.GetValueOrDefault(bar) : null;
        }

        return field.StepNumber is { } step ? rowData.GetValueOrDefault(step) : null;
    }

    private static object ConvertRaw(int? raw, string type)
    {
        return type switch
        {
            "bool" => raw.GetValueOrDefault() != 0,
            "int" => raw.GetValueOrDefault(),
            _ => raw.GetValueOrDefault(),
        };
    }

    private readonly record struct FieldConfig(int? StepNumber, int? BarNumber, string Type)
    {
        public bool IsBar => StepNumber is null && BarNumber is not null;

        public static bool TryCreate(object? value, out FieldConfig config)
        {
            config = default;
            var map = MiniYaml.AsMap(value);
            if (map is null || !map.ContainsKey("step"))
            {
                return false;
            }

            var stepValue = map.GetValueOrDefault("step");
            var stepString = MiniYaml.AsString(stepValue);
            var type = MiniYaml.AsString(map.GetValueOrDefault("type")) ?? "int";

            if (string.Equals(stepString, "bar", StringComparison.OrdinalIgnoreCase))
            {
                config = new FieldConfig(null, MiniYaml.AsInt(map.GetValueOrDefault("bar")), type);
                return true;
            }

            config = new FieldConfig(MiniYaml.AsInt(stepValue), null, type);
            return config.StepNumber is not null;
        }
    }
}
