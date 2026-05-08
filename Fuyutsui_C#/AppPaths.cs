namespace FuyutsuiCSharp;

public static class AppPaths
{
    public static DirectoryInfo FindCSharpProjectDirectory()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "FuyutsuiCSharp.csproj")))
            {
                return dir;
            }

            var nested = Path.Combine(dir.FullName, "Fuyutsui_C#", "FuyutsuiCSharp.csproj");
            if (File.Exists(nested))
            {
                return new DirectoryInfo(Path.Combine(dir.FullName, "Fuyutsui_C#"));
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("未找到 Fuyutsui_C# 项目目录。");
    }

    public static DirectoryInfo FindCSharpDataDirectory(bool create = false)
    {
        var dataDir = new DirectoryInfo(Path.Combine(FindCSharpProjectDirectory().FullName, "Data"));
        if (create && !dataDir.Exists)
        {
            dataDir.Create();
        }

        return dataDir;
    }

    public static DirectoryInfo FindPythonDataDirectory()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, "Fuyutsui");
            if (File.Exists(Path.Combine(candidate, "config.yml")))
            {
                return new DirectoryInfo(candidate);
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("未找到包含 config.yml 的 Fuyutsui 数据目录。");
    }
}
