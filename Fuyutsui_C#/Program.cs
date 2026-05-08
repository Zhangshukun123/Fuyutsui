using System.Text.Encodings.Web;
using System.Text.Json;
using FuyutsuiCSharp;

NativeMethods.TrySetProcessDpiAware();

var command = args.FirstOrDefault()?.ToLowerInvariant();

switch (command)
{
    case "scan":
        PrintScan(new FuyutsuiRuntime());
        return;
    case "watch":
        Watch(new FuyutsuiRuntime());
        return;
    case "hotkey":
        PrintHotkey(new FuyutsuiRuntime(), args);
        return;
    case "send":
        SendHotkey(args);
        return;
    case "export-data":
        ExportData();
        return;
    case "gui":
    case null:
    case "":
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(new FuyutsuiRuntime()));
        return;
    default:
        PrintHelp();
        return;
}

static void ExportData()
{
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    DataMigrator.ExportYamlToJson();
    Console.WriteLine("已生成 Fuyutsui_C#/Data/config.json 和 Data/keymaps/*.json");
}

static void PrintScan(FuyutsuiRuntime runtime)
{
    var started = DateTime.UtcNow;
    var info = runtime.GetInfo();
    var elapsed = (DateTime.UtcNow - started).TotalMilliseconds;
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    Console.WriteLine($"扫描耗时: {elapsed:F2} ms");
    if (info is null)
    {
        Console.WriteLine("未找到游戏窗口或扫描失败");
        return;
    }

    Console.WriteLine(JsonSerializer.Serialize(info, JsonOptions()));
}

static void Watch(FuyutsuiRuntime runtime)
{
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    while (true)
    {
        Console.Clear();
        PrintScan(runtime);
        Thread.Sleep(200);
    }
}

static void PrintHotkey(FuyutsuiRuntime runtime, string[] args)
{
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    if (args.Length < 3 || !int.TryParse(args[1], out var unit))
    {
        Console.WriteLine("用法: dotnet run -- hotkey <unit> <spell>");
        return;
    }

    var spell = string.Join(' ', args.Skip(2));
    var info = runtime.GetInfo();
    var classId = info.GetInt("职业");
    runtime.Keymaps.SelectKeymapForClass(classId);
    Console.WriteLine(runtime.Keymaps.GetHotkey(unit, spell) ?? "(未找到)");
}

static void SendHotkey(string[] args)
{
    if (args.Length < 2)
    {
        Console.WriteLine("用法: dotnet run -- send <hotkey>");
        return;
    }

    var hotkey = string.Join(' ', args.Skip(1));
    Console.WriteLine(WowInput.SendKeyToWow(hotkey) ? "已发送" : "发送失败");
}

static JsonSerializerOptions JsonOptions() => new()
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
};

static void PrintHelp()
{
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    Console.WriteLine("""
    Fuyutsui C# 用法:
      dotnet run --project Fuyutsui_C# -- gui
      dotnet run --project Fuyutsui_C# -- scan
      dotnet run --project Fuyutsui_C# -- watch
      dotnet run --project Fuyutsui_C# -- hotkey <unit> <spell>
      dotnet run --project Fuyutsui_C# -- send <hotkey>
      dotnet run --project Fuyutsui_C# -- export-data
    """);
}
