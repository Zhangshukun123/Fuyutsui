# Fuyutsui C#

这是 `Fuyutsui/` Python 工具的 C# 迁移版起点，当前已实现：

- Win32 客户区定位和 DPI 适配
- 顶部长条像素扫描
- 左侧红/白标记条扫描
- `Data/config.json` 解码为状态字典
- `Data/keymaps/*.json` 职业 keymap 选择与热键查询
- `PostMessageW` 后台发送热键
- 一个轻量 WinForms 实时状态窗口

## 运行

```powershell
dotnet run --project .\Fuyutsui_C# -- gui
dotnet run --project .\Fuyutsui_C# -- scan
dotnet run --project .\Fuyutsui_C# -- watch
dotnet run --project .\Fuyutsui_C# -- hotkey 1 苦修
dotnet run --project .\Fuyutsui_C# -- send CTRL-NUMPAD1
dotnet run --project .\Fuyutsui_C# -- export-data
```

`export-data` 会从旧版 `Fuyutsui/config.yml` 和 `Fuyutsui/keymap/*.yml` 生成 C# 运行时优先使用的 JSON 数据。

职业逻辑 (`Fuyutsui/class/*_logic.py`) 还没有逐个翻译到 C#。建议下一步按职业拆成 `ILogicRunner` 实现，先迁移当前最常用的专精。
