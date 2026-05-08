using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FuyutsuiCSharp;

public sealed class PixelScanner
{
    private const int PixelsPerRow = 255;

    public ScanResult ScanScreenData(string windowTitle = "魔兽世界")
    {
        var hwnd = NativeMethods.FindWindow(null, windowTitle);
        if (hwnd == IntPtr.Zero || NativeMethods.IsIconic(hwnd))
        {
            return ScanResult.Empty;
        }

        var point = new NativeMethods.Point();
        if (!NativeMethods.ClientToScreen(hwnd, ref point) ||
            !NativeMethods.GetClientRect(hwnd, out var rect))
        {
            return ScanResult.Empty;
        }

        var width = rect.Right - rect.Left;
        var height = rect.Bottom - rect.Top;
        if (width <= 0 || height <= 0)
        {
            return ScanResult.Empty;
        }

        try
        {
            var rowData = ScanTopRow(point.X, point.Y, width);
            var barData = ScanLeftMarkerRow(point.X, point.Y, width, height);
            return new ScanResult(rowData.Count > 0 ? rowData : null, barData);
        }
        catch
        {
            return ScanResult.Empty;
        }
    }

    private static Dictionary<int, int> ScanTopRow(int baseX, int baseY, int width)
    {
        var rowData = new Dictionary<int, int>();
        var raw = CaptureRaw(baseX, baseY, width, 1, out var stride);
        var startX = -1;

        for (var x = 0; x < Math.Min(PixelsPerRow, width); x++)
        {
            var offset = Offset(0, x, stride, 1);
            var b = raw[offset];
            var g = raw[offset + 1];
            var r = raw[offset + 2];
            if (IsGreenMarker(b, g, r))
            {
                startX = x;
                break;
            }
        }

        if (startX == -1)
        {
            return rowData;
        }

        for (var x = startX; x < width; x++)
        {
            var offset = Offset(0, x, stride, 1);
            var b = raw[offset];
            var g = raw[offset + 1];
            var r = raw[offset + 2];
            if (r == 0 && g is >= 1 and <= PixelsPerRow)
            {
                rowData[g] = b;
                if (g == PixelsPerRow)
                {
                    break;
                }
            }
            else if (g > PixelsPerRow)
            {
                break;
            }
        }

        return rowData;
    }

    private static Dictionary<int, int> ScanLeftMarkerRow(int baseX, int baseY, int width, int height)
    {
        var barData = new Dictionary<int, int>();
        var leftRaw = CaptureRaw(baseX, baseY, 1, height, out var leftStride);
        int? markerY = null;

        for (var y = 0; y < height; y++)
        {
            var offset = Offset(y, 0, leftStride, height);
            var b = leftRaw[offset];
            var g = leftRaw[offset + 1];
            var r = leftRaw[offset + 2];
            if (IsRedMarker(b, g, r))
            {
                markerY = y;
                break;
            }
        }

        if (markerY is null)
        {
            return barData;
        }

        var raw = CaptureRaw(baseX, baseY + markerY.Value, width, 1, out var stride);
        var segmentIndex = 0;
        var x = 0;
        var pendingRed = false;

        while (x < width)
        {
            var offset = Offset(0, x, stride, 1);
            var b = raw[offset];
            var g = raw[offset + 1];
            var r = raw[offset + 2];

            if (pendingRed && IsRedGreenMarker(b, g, r))
            {
                pendingRed = false;
                segmentIndex++;
                var (value, nextX) = ConsumeValue(raw, stride, width, x + 1, alreadySawWhite: false);
                barData[segmentIndex] = Math.Max(0, value - 1);
                x = nextX;
                continue;
            }

            if (IsRedMarker(b, g, r))
            {
                pendingRed = true;
                x++;
                continue;
            }

            if (IsWhite(b, g, r))
            {
                var previousWhite = false;
                if (x > 0)
                {
                    var prevOffset = Offset(0, x - 1, stride, 1);
                    previousWhite = IsWhite(raw[prevOffset], raw[prevOffset + 1], raw[prevOffset + 2]);
                }

                if (!previousWhite)
                {
                    pendingRed = false;
                    segmentIndex++;
                    var (value, nextX) = ConsumeValue(raw, stride, width, x + 1, alreadySawWhite: true);
                    barData[segmentIndex] = Math.Max(0, value - 1);
                    x = nextX;
                    continue;
                }
            }

            x++;
        }

        return barData;
    }

    private static (int Value, int NextX) ConsumeValue(byte[] raw, int stride, int width, int fromX, bool alreadySawWhite)
    {
        var x = fromX;
        var needWhite = !alreadySawWhite;
        while (x < width)
        {
            var offset = Offset(0, x, stride, 1);
            var b = raw[offset];
            var g = raw[offset + 1];
            var r = raw[offset + 2];

            if (IsRedMarker(b, g, r))
            {
                return (0, x);
            }

            if (needWhite)
            {
                if (IsWhite(b, g, r))
                {
                    needWhite = false;
                }

                x++;
                continue;
            }

            if (IsWhite(b, g, r))
            {
                x++;
                continue;
            }

            return (g, x + 1);
        }

        return (0, width);
    }

    private static byte[] CaptureRaw(int x, int y, int width, int height, out int stride)
    {
        using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using (var graphics = Graphics.FromImage(bmp))
        {
            graphics.CopyFromScreen(x, y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
        }

        var rect = new Rectangle(0, 0, width, height);
        var data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        try
        {
            stride = data.Stride;
            var bytes = Math.Abs(stride) * height;
            var raw = new byte[bytes];
            Marshal.Copy(data.Scan0, raw, 0, bytes);
            return raw;
        }
        finally
        {
            bmp.UnlockBits(data);
        }
    }

    private static int Offset(int row, int column, int stride, int height)
    {
        if (stride >= 0)
        {
            return row * stride + column * 4;
        }

        return (height - 1 - row) * Math.Abs(stride) + column * 4;
    }

    private static bool IsRedMarker(byte b, byte g, byte r) => r == 1 && g == 0 && b == 0;
    private static bool IsRedGreenMarker(byte b, byte g, byte r) => r == 1 && g == 1 && b == 0;
    private static bool IsWhite(byte b, byte g, byte r) => r == 255 && g == 255 && b == 255;
    private static bool IsGreenMarker(byte b, byte g, byte r) => r == 0 && g == 1 && b == 0;
}

public sealed record ScanResult(Dictionary<int, int>? RowData, Dictionary<int, int> BarData)
{
    public static ScanResult Empty { get; } = new(null, new Dictionary<int, int>());
}
