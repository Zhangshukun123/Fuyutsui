using System.Globalization;
using System.Text;

namespace FuyutsuiCSharp;

public static class MiniYaml
{
    public static Dictionary<string, object?> LoadFile(string path)
    {
        var root = new Dictionary<string, object?>(StringComparer.Ordinal);
        var stack = new Stack<(int Indent, Dictionary<string, object?> Map)>();
        stack.Push((-1, root));

        foreach (var rawLine in File.ReadLines(path))
        {
            var lineWithoutComments = StripComment(rawLine);
            if (string.IsNullOrWhiteSpace(lineWithoutComments))
            {
                continue;
            }

            var indent = CountLeadingSpaces(lineWithoutComments);
            var line = lineWithoutComments.Trim();
            var colon = FindTopLevelColon(line);
            if (colon <= 0)
            {
                continue;
            }

            while (stack.Count > 1 && indent <= stack.Peek().Indent)
            {
                stack.Pop();
            }

            var key = Unquote(line[..colon].Trim());
            var valueText = line[(colon + 1)..].Trim();
            var parent = stack.Peek().Map;

            if (valueText.Length == 0)
            {
                var child = new Dictionary<string, object?>(StringComparer.Ordinal);
                parent[key] = child;
                stack.Push((indent, child));
            }
            else
            {
                parent[key] = ParseValue(valueText);
            }
        }

        return root;
    }

    public static Dictionary<string, object?>? AsMap(object? value)
    {
        return value as Dictionary<string, object?>;
    }

    public static int? AsInt(object? value)
    {
        return value switch
        {
            int i => i,
            long l => (int)l,
            string s when int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) => i,
            _ => null,
        };
    }

    public static string? AsString(object? value)
    {
        return value switch
        {
            null => null,
            string s => s,
            int i => i.ToString(CultureInfo.InvariantCulture),
            _ => value.ToString(),
        };
    }

    private static object? ParseValue(string text)
    {
        text = text.Trim();
        if (text.Length == 0)
        {
            return "";
        }

        if (text.StartsWith('{') && text.EndsWith('}'))
        {
            return ParseInlineMap(text[1..^1]);
        }

        if (text.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (text.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
        {
            return i;
        }

        return Unquote(text);
    }

    private static Dictionary<string, object?> ParseInlineMap(string content)
    {
        var map = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var part in SplitTopLevel(content, ','))
        {
            var trimmed = part.Trim();
            if (trimmed.Length == 0)
            {
                continue;
            }

            var colon = FindTopLevelColon(trimmed);
            if (colon <= 0)
            {
                continue;
            }

            var key = Unquote(trimmed[..colon].Trim());
            var value = trimmed[(colon + 1)..].Trim();
            map[key] = ParseValue(value);
        }

        return map;
    }

    private static IEnumerable<string> SplitTopLevel(string input, char delimiter)
    {
        var start = 0;
        var quote = '\0';
        var braceDepth = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if (quote != '\0')
            {
                if (ch == quote && (i == 0 || input[i - 1] != '\\'))
                {
                    quote = '\0';
                }
                continue;
            }

            if (ch is '"' or '\'')
            {
                quote = ch;
                continue;
            }

            if (ch == '{')
            {
                braceDepth++;
                continue;
            }

            if (ch == '}')
            {
                braceDepth--;
                continue;
            }

            if (ch == delimiter && braceDepth == 0)
            {
                yield return input[start..i];
                start = i + 1;
            }
        }

        yield return input[start..];
    }

    private static int FindTopLevelColon(string input)
    {
        var quote = '\0';
        var braceDepth = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if (quote != '\0')
            {
                if (ch == quote && (i == 0 || input[i - 1] != '\\'))
                {
                    quote = '\0';
                }
                continue;
            }

            if (ch is '"' or '\'')
            {
                quote = ch;
                continue;
            }

            if (ch == '{')
            {
                braceDepth++;
                continue;
            }

            if (ch == '}')
            {
                braceDepth--;
                continue;
            }

            if (ch == ':' && braceDepth == 0)
            {
                return i;
            }
        }

        return -1;
    }

    private static string StripComment(string input)
    {
        var sb = new StringBuilder(input.Length);
        var quote = '\0';
        for (var i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if (quote != '\0')
            {
                sb.Append(ch);
                if (ch == quote && (i == 0 || input[i - 1] != '\\'))
                {
                    quote = '\0';
                }
                continue;
            }

            if (ch is '"' or '\'')
            {
                quote = ch;
                sb.Append(ch);
                continue;
            }

            if (ch == '#')
            {
                break;
            }

            sb.Append(ch);
        }

        return sb.ToString();
    }

    private static int CountLeadingSpaces(string input)
    {
        var count = 0;
        while (count < input.Length && input[count] == ' ')
        {
            count++;
        }

        return count;
    }

    private static string Unquote(string value)
    {
        value = value.Trim();
        if (value.Length >= 2 && ((value[0] == '"' && value[^1] == '"') || (value[0] == '\'' && value[^1] == '\'')))
        {
            return value[1..^1].Replace("\\\"", "\"", StringComparison.Ordinal);
        }

        return value;
    }
}
