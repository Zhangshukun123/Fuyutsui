using System.Text.Encodings.Web;
using System.Text.Json;

namespace FuyutsuiCSharp;

public static class JsonData
{
    private static readonly JsonSerializerOptions WriteOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static Dictionary<string, object?> LoadDictionary(string path)
    {
        var value = LoadObject(path);
        return value as Dictionary<string, object?>
            ?? throw new InvalidDataException($"JSON 根节点必须是对象: {path}");
    }

    public static object? LoadObject(string path)
    {
        using var document = JsonDocument.Parse(File.ReadAllText(path));
        return ConvertElement(document.RootElement);
    }

    public static void SaveObject(string path, object? value)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path) ?? ".");
        File.WriteAllText(path, JsonSerializer.Serialize(value, WriteOptions));
    }

    private static object? ConvertElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => ConvertObject(element),
            JsonValueKind.Array => element.EnumerateArray().Select(ConvertElement).ToList(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt32(out var i) => i,
            JsonValueKind.Number when element.TryGetInt64(out var l) => l,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => null,
        };
    }

    private static Dictionary<string, object?> ConvertObject(JsonElement element)
    {
        var map = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var property in element.EnumerateObject())
        {
            map[property.Name] = ConvertElement(property.Value);
        }

        return map;
    }
}
