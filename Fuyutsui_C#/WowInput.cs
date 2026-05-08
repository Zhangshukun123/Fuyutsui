namespace FuyutsuiCSharp;

public static class WowInput
{
    private const uint WmKeyDown = 0x0100;
    private const uint WmKeyUp = 0x0101;

    private static readonly Dictionary<string, int> Vk = new(StringComparer.OrdinalIgnoreCase)
    {
        ["SHIFT"] = 0x10,
        ["CONTROL"] = 0x11,
        ["CTRL"] = 0x11,
        ["MENU"] = 0x12,
        ["ALT"] = 0x12,
        ["XBUTTON1"] = 0x05,
        ["X1"] = 0x05,
        ["MOUSE4"] = 0x05,
        ["XBUTTON2"] = 0x06,
        ["X2"] = 0x06,
        ["MOUSE5"] = 0x06,
        ["F1"] = 0x70,
        ["F2"] = 0x71,
        ["F3"] = 0x72,
        ["F4"] = 0x73,
        ["F5"] = 0x74,
        ["F6"] = 0x75,
        ["F7"] = 0x76,
        ["F8"] = 0x77,
        ["F9"] = 0x78,
        ["F10"] = 0x79,
        ["F11"] = 0x7A,
        ["F12"] = 0x7B,
        ["NUMPAD0"] = 0x60,
        ["NUMPAD1"] = 0x61,
        ["NUMPAD2"] = 0x62,
        ["NUMPAD3"] = 0x63,
        ["NUMPAD4"] = 0x64,
        ["NUMPAD5"] = 0x65,
        ["NUMPAD6"] = 0x66,
        ["NUMPAD7"] = 0x67,
        ["NUMPAD8"] = 0x68,
        ["NUMPAD9"] = 0x69,
        ["NUMPADDECIMAL"] = 0x6E,
        ["NUMPADPLUS"] = 0x6B,
        ["NUMPADMINUS"] = 0x6D,
        ["NUMPADMULTIPLY"] = 0x6A,
        ["NUMPADDIVIDE"] = 0x6F,
        [","] = 0xBC,
        ["."] = 0xBE,
        ["/"] = 0xBF,
        [";"] = 0xBA,
        ["'"] = 0xDE,
        ["["] = 0xDB,
        ["]"] = 0xDD,
        ["="] = 0xBB,
        ["-"] = 0xBD,
        ["`"] = 0xC0,
    };

    public static int? GetVirtualKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return null;
        }

        key = key.Trim();
        if (Vk.TryGetValue(key, out var mapped))
        {
            return mapped;
        }

        if (key.Length == 1)
        {
            var vk = NativeMethods.VkKeyScanW(key[0]);
            return vk == -1 ? null : vk & 0xff;
        }

        return null;
    }

    public static bool SendKeyToWow(string hotkey, string windowTitle = "魔兽世界")
    {
        var (mods, main) = ParseHotkey(hotkey);
        if (main is null)
        {
            return false;
        }

        var mainVk = GetVirtualKey(main);
        if (mainVk is null)
        {
            return false;
        }

        var hwnd = NativeMethods.FindWindow(null, windowTitle);
        if (hwnd == IntPtr.Zero)
        {
            return false;
        }

        var modVks = mods.Select(GetVirtualKey).Where(v => v is not null).Select(v => v!.Value).Distinct().ToList();
        foreach (var vk in modVks)
        {
            Post(hwnd, vk, keyUp: false);
        }

        Post(hwnd, mainVk.Value, keyUp: false);
        Post(hwnd, mainVk.Value, keyUp: true);

        for (var i = modVks.Count - 1; i >= 0; i--)
        {
            Post(hwnd, modVks[i], keyUp: true);
        }

        return true;
    }

    private static (List<string> Mods, string? Main) ParseHotkey(string hotkey)
    {
        if (string.IsNullOrWhiteSpace(hotkey))
        {
            return ([], null);
        }

        var parts = hotkey.Trim().Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0)
        {
            return ([], null);
        }

        var mods = new List<string>();
        foreach (var part in parts.Take(parts.Length - 1))
        {
            var normalized = part.ToUpperInvariant() switch
            {
                "CONTROL" => "CTRL",
                "MENU" => "ALT",
                "CTRL" or "ALT" or "SHIFT" => part.ToUpperInvariant(),
                _ => null,
            };

            if (normalized is not null && !mods.Contains(normalized, StringComparer.OrdinalIgnoreCase))
            {
                mods.Add(normalized);
            }
        }

        return (mods, parts[^1]);
    }

    private static void Post(IntPtr hwnd, int keyCode, bool keyUp)
    {
        var lParam = keyUp ? unchecked((int)0xC0000001) : 0x00000001;
        NativeMethods.PostMessageW(hwnd, keyUp ? WmKeyUp : WmKeyDown, keyCode, lParam);
    }
}
