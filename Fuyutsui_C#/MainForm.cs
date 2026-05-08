using System.Diagnostics;
using System.Text.Json;

namespace FuyutsuiCSharp;

public sealed class MainForm : Form
{
    private readonly FuyutsuiRuntime _runtime;
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 200 };
    private readonly Label _classLabel = new();
    private readonly Label _specLabel = new();
    private readonly Label _statusLabel = new();
    private readonly Label _scanLabel = new();
    private readonly TextBox _stateBox = new();
    private bool _scanInFlight;

    public MainForm(FuyutsuiRuntime runtime)
    {
        _runtime = runtime;
        Text = "冬月";
        Width = 460;
        Height = 360;
        MinimumSize = new Size(360, 220);
        TopMost = true;
        BackColor = Color.FromArgb(30, 30, 30);
        ForeColor = Color.Gainsboro;
        Font = new Font("Microsoft YaHei UI", 9F);

        var top = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 92,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new Padding(12, 10, 12, 4),
        };
        top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        ConfigureLabel(_classLabel, "职业: -", 13, bold: true);
        ConfigureLabel(_specLabel, "专精: -", 13, bold: true);
        ConfigureLabel(_statusLabel, "状态: 等待扫描", 10);
        ConfigureLabel(_scanLabel, "扫描: - ms", 10);

        top.Controls.Add(_classLabel, 0, 0);
        top.Controls.Add(_specLabel, 1, 0);
        top.Controls.Add(_statusLabel, 0, 1);
        top.Controls.Add(_scanLabel, 1, 1);

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
        };

        var scanButton = BuildButton("扫描一次");
        scanButton.Click += async (_, _) => await RefreshStateAsync();
        var copyButton = BuildButton("复制状态");
        copyButton.Click += (_, _) =>
        {
            if (!string.IsNullOrWhiteSpace(_stateBox.Text))
            {
                Clipboard.SetText(_stateBox.Text);
            }
        };
        var closeButton = BuildButton("退出");
        closeButton.Click += (_, _) => Close();

        buttonPanel.Controls.Add(scanButton);
        buttonPanel.Controls.Add(copyButton);
        buttonPanel.Controls.Add(closeButton);
        top.SetColumnSpan(buttonPanel, 2);
        top.Controls.Add(buttonPanel, 0, 2);

        _stateBox.Dock = DockStyle.Fill;
        _stateBox.Multiline = true;
        _stateBox.ScrollBars = ScrollBars.Both;
        _stateBox.WordWrap = false;
        _stateBox.BackColor = Color.FromArgb(24, 24, 24);
        _stateBox.ForeColor = Color.Gainsboro;
        _stateBox.BorderStyle = BorderStyle.FixedSingle;
        _stateBox.Font = new Font("Consolas", 9F);

        Controls.Add(_stateBox);
        Controls.Add(top);

        _timer.Tick += async (_, _) => await RefreshStateAsync();
        Shown += async (_, _) =>
        {
            _timer.Start();
            await RefreshStateAsync();
        };
        FormClosing += (_, _) => _timer.Stop();
    }

    private async Task RefreshStateAsync()
    {
        if (_scanInFlight)
        {
            return;
        }

        _scanInFlight = true;
        try
        {
            var sw = Stopwatch.StartNew();
            var info = await Task.Run(() => _runtime.GetInfo());
            sw.Stop();

            _scanLabel.Text = $"扫描: {sw.Elapsed.TotalMilliseconds:F1} ms";
            if (info is null)
            {
                _classLabel.Text = "职业: -";
                _specLabel.Text = "专精: -";
                _statusLabel.Text = "状态: 未找到游戏窗口";
                _statusLabel.ForeColor = Color.FromArgb(255, 107, 107);
                return;
            }

            var classId = info.GetInt("职业");
            var specId = info.GetInt("专精");
            var (className, specName) = ClassSpecNames.Get(classId, specId);
            _classLabel.Text = $"职业: {className ?? "-"}";
            _specLabel.Text = $"专精: {specName ?? "-"}";
            var valid = info.GetBool("有效性");
            var combat = info.GetBool("战斗");
            _statusLabel.Text = $"状态: {(valid ? "有效" : "无效")} / {(combat ? "战斗" : "脱战")}";
            _statusLabel.ForeColor = valid ? Color.FromArgb(0, 217, 165) : Color.FromArgb(255, 107, 107);
            _stateBox.Text = JsonSerializer.Serialize(info, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            });
        }
        finally
        {
            _scanInFlight = false;
        }
    }

    private static void ConfigureLabel(Label label, string text, float size, bool bold = false)
    {
        label.Text = text;
        label.Dock = DockStyle.Fill;
        label.AutoSize = false;
        label.ForeColor = Color.Gainsboro;
        label.Font = new Font("Microsoft YaHei UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
        label.TextAlign = ContentAlignment.MiddleLeft;
    }

    private static Button BuildButton(string text)
    {
        return new Button
        {
            Text = text,
            AutoSize = true,
            Height = 28,
            BackColor = Color.FromArgb(45, 45, 45),
            ForeColor = Color.Gainsboro,
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(0, 2, 8, 2),
        };
    }
}
