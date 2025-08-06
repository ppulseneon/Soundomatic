using System;
using System.Runtime.InteropServices;
using Avalonia;
using Microsoft.Extensions.Logging;

namespace Soundomatic.Helpers;

/// <summary>
/// Помощник для интеллектуального позиционирования окна трея
/// </summary>
public static class TrayPositioningHelper
{
    private const int DefaultTrayOffset = 5;

    /// <summary>
    /// Структура для получения информации о рабочей области экрана (Windows)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    /// <summary>
    /// Получение информации о мониторе по точке
    /// </summary>
    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

    /// <summary>
    /// Получение информации о мониторе
    /// </summary>
    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    /// <summary>
    /// Получение позиции курсора для Windows
    /// </summary>
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    /// <summary>
    /// DLL для получения позиции курсора для Linux
    /// </summary>
    [DllImport("libX11")]
    private static extern IntPtr XOpenDisplay(IntPtr display);

    /// <summary>
    /// DLL для получения позиции курсора для Linux
    /// </summary>
    [DllImport("libX11")]
    private static extern int XQueryPointer(IntPtr display, IntPtr w, out IntPtr root_return,
        out System.IntPtr child_return, out int root_x_return, out int root_y_return,
        out int win_x_return, out int win_y_return, out uint mask_return);

    /// <summary>
    /// DLL для получения позиции курсора для MacOS
    /// </summary>
    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern IntPtr CGEventCreate(IntPtr source);

    [DllImport("/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics")]
    private static extern CGPoint CGEventGetLocation(IntPtr eventRef);

    /// <summary>
    /// Структура позиции для MacOS
    /// </summary>
    private struct CGPoint
    {
        public double X;
        public double Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }

    /// <summary>
    /// Получает текущую позицию курсора
    /// </summary>
    /// <returns>Позиция курсора в пикселях</returns>
    public static PixelPoint GetCursorPosition()
    {
        var position = GetCursorPositionInternal();
        return new PixelPoint(position.X, position.Y);
    }

    /// <summary>
    /// Внутренний метод для получения позиции курсора на разных платформах
    /// </summary>
    private static (int X, int Y) GetCursorPositionInternal()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (GetCursorPos(out var lpPoint))
                return (lpPoint.X, lpPoint.Y);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return GetCursorPositionMacOs();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return GetCursorPositionX11();
        }

        return (0, 0);
    }

    /// <summary>
    /// Получение позиции курсора для Linux
    /// </summary>
    private static (int X, int Y) GetCursorPositionX11()
    {
        IntPtr display = XOpenDisplay(IntPtr.Zero);
        if (display == IntPtr.Zero)
            return (100, 100);

        IntPtr root, child;
        uint mask;
        int root_x, root_y, win_x, win_y;

        XQueryPointer(display, IntPtr.Zero, out root, out child, out root_x, out root_y, out win_x, out win_y, out mask);
        return (root_x, root_y);
    }

    /// <summary>
    /// Получение позиции курсора для MacOS
    /// </summary>
    private static (int X, int Y) GetCursorPositionMacOs()
    {
        var eventRef = CGEventCreate(IntPtr.Zero);
        var loc = CGEventGetLocation(eventRef);
        return ((int)loc.X, (int)loc.Y);
    }

    /// <summary>
    /// Вычисляет оптимальную позицию для окна трея
    /// </summary>
    /// <param name="cursorPosition">Позиция курсора</param>
    /// <param name="windowSize">Размер окна</param>
    /// <param name="logger">Логгер для отладки</param>
    /// <returns>Оптимальная позиция окна</returns>
    public static PixelPoint CalculateOptimalPosition(PixelPoint cursorPosition, Size windowSize, ILogger? logger = null)
    {
        try
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? CalculateWindowsPosition(cursorPosition, windowSize, logger) : CalculateCrossPlatformPosition(cursorPosition, windowSize, logger);
        }
        catch (Exception ex)
        {
            logger?.LogWarning(ex, "Ошибка при вычислении позиции окна трея, используется fallback позиция");
            return CalculateFallbackPosition(cursorPosition, windowSize);
        }
    }

    /// <summary>
    /// Вычисляет позицию для Windows с учетом рабочей области
    /// </summary>
    private static PixelPoint CalculateWindowsPosition(PixelPoint cursorPosition, Size windowSize, ILogger? logger)
    {
        var cursorPoint = new POINT { X = cursorPosition.X, Y = cursorPosition.Y };
        var hMonitor = MonitorFromPoint(cursorPoint, 0x00000002); // MONITOR_DEFAULTTONEAREST

        var monitorInfo = new MONITORINFO { cbSize = (uint)Marshal.SizeOf<MONITORINFO>() };
        
        if (GetMonitorInfo(hMonitor, ref monitorInfo))
        {
            var workArea = monitorInfo.rcWork;
            var windowWidth = (int)windowSize.Width;
            var windowHeight = (int)windowSize.Height;

            var taskbarLocation = DetectTaskbarLocation(monitorInfo);
            
            var finalX = CalculateXPosition(cursorPosition.X, windowWidth, workArea, taskbarLocation);
            var finalY = CalculateYPosition(cursorPosition.Y, windowHeight, workArea, taskbarLocation);

            return new PixelPoint(finalX, finalY);
        }

        logger?.LogWarning("Не удалось получить информацию о мониторе, используется fallback");
        return CalculateFallbackPosition(cursorPosition, windowSize);
    }

    /// <summary>
    /// Определяет расположение панели задач
    /// </summary>
    private static TaskbarLocation DetectTaskbarLocation(MONITORINFO monitorInfo)
    {
        var monitor = monitorInfo.rcMonitor;
        var work = monitorInfo.rcWork;

        // Панель задач снизу (наиболее частый случай)
        if (work.Bottom < monitor.Bottom && work.Top == monitor.Top 
            && work.Left == monitor.Left && work.Right == monitor.Right)
            return TaskbarLocation.Bottom;

        // Панель задач сверху
        if (work.Top > monitor.Top && work.Bottom == monitor.Bottom 
            && work.Left == monitor.Left && work.Right == monitor.Right)
            return TaskbarLocation.Top;

        // Панель задач слева
        if (work.Left > monitor.Left && work.Top == monitor.Top 
            && work.Bottom == monitor.Bottom && work.Right == monitor.Right)
            return TaskbarLocation.Left;

        // Панель задач справа
        if (work.Right < monitor.Right && work.Top == monitor.Top 
            && work.Bottom == monitor.Bottom && work.Left == monitor.Left)
            return TaskbarLocation.Right;

        // По умолчанию считаем, что снизу
        return TaskbarLocation.Bottom;
    }

    /// <summary>
    /// Вычисляет X координату окна
    /// </summary>
    private static int CalculateXPosition(int cursorX, int windowWidth, RECT workArea, TaskbarLocation taskbarLocation)
    {
        // Пытаемся разместить окно слева от курсора
        var preferredX = cursorX - windowWidth - DefaultTrayOffset;

        // Проверяем, помещается ли окно в рабочую область
        if (preferredX >= workArea.Left)
        {
            return preferredX;
        }

        // Если не помещается слева, размещаем справа от курсора
        var alternativeX = cursorX + DefaultTrayOffset;
        
        return alternativeX + windowWidth <= workArea.Right ? alternativeX :
            // В крайнем случае прижимаем к правому краю рабочей области
            Math.Max(workArea.Left, workArea.Right - windowWidth - DefaultTrayOffset);
    }

    /// <summary>
    /// Вычисляет Y координату окна
    /// </summary>
    private static int CalculateYPosition(int cursorY, int windowHeight, RECT workArea, TaskbarLocation taskbarLocation)
    {
        switch (taskbarLocation)
        {
            case TaskbarLocation.Bottom:
                // Панель задач снизу - размещаем окно над курсором
                var preferredY = cursorY - windowHeight - DefaultTrayOffset;
                return Math.Max(workArea.Top, Math.Min(preferredY, workArea.Bottom - windowHeight));

            case TaskbarLocation.Top:
                // Панель задач сверху - размещаем окно под курсором
                var preferredYTop = cursorY + DefaultTrayOffset;
                return Math.Max(workArea.Top, Math.Min(preferredYTop, workArea.Bottom - windowHeight));

            case TaskbarLocation.Left:
            case TaskbarLocation.Right:
                // Панель задач сбоку - размещаем окно по центру относительно курсора
                var centerY = cursorY - windowHeight / 2;
                return Math.Max(workArea.Top, Math.Min(centerY, workArea.Bottom - windowHeight));

            default:
                return Math.Max(workArea.Top, cursorY - windowHeight - DefaultTrayOffset);
        }
    }

    /// <summary>
    /// Кроссплатформенное вычисление позиции (для Linux/macOS)
    /// </summary>
    private static PixelPoint CalculateCrossPlatformPosition(PixelPoint cursorPosition, Size windowSize, ILogger? logger)
    {
        // Для других платформ используем более простой подход
        // Получаем размеры экрана через Avalonia
        var screens = Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow?.Screens
            : null;

        var currentScreen = screens?.ScreenFromPoint(cursorPosition) ?? screens?.Primary;
        
        if (currentScreen == null) return CalculateFallbackPosition(cursorPosition, windowSize);
        
        var workingArea = currentScreen.WorkingArea;
        var windowWidth = (int)windowSize.Width;
        var windowHeight = (int)windowSize.Height;

        // Простая логика: окно слева-сверху от курсора с проверкой границ
        var finalX = Math.Max(workingArea.X, 
            Math.Min(cursorPosition.X - windowWidth - DefaultTrayOffset, 
            workingArea.Right - windowWidth));

        var finalY = Math.Max(workingArea.Y, 
            Math.Min(cursorPosition.Y - windowHeight - DefaultTrayOffset, 
            workingArea.Bottom - windowHeight));

        return new PixelPoint(finalX, finalY);

    }

    /// <summary>
    /// Резервная позиция в случае ошибки
    /// </summary>
    private static PixelPoint CalculateFallbackPosition(PixelPoint cursorPosition, Size windowSize)
    {
        return new PixelPoint(
            Math.Max(0, cursorPosition.X - (int)windowSize.Width - DefaultTrayOffset),
            Math.Max(0, cursorPosition.Y - (int)windowSize.Height - DefaultTrayOffset)
        );
    }

    /// <summary>
    /// Расположение панели задач
    /// </summary>
    private enum TaskbarLocation
    {
        Bottom,
        Top,
        Left,
        Right
    }
} 