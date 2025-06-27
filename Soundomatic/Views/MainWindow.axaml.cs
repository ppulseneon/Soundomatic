using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Soundomatic.Services.Interfaces;

namespace Soundomatic.Views;

/// <summary>
/// Основное окно приложения
/// </summary>
public partial class MainWindow : Window
{
    private readonly ISystemNotificationService _systemNotificationService;
    
    public MainWindow(ISystemNotificationService systemNotificationService)
    {
        _systemNotificationService = systemNotificationService;
        InitializeComponent();
    }

    /// <summary>
    /// Метод для обработки зажатия содержимого окна
    /// </summary>
    private void Content_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.Handled = true;
    }

    /// <summary>
    /// Метод для обработки зажатия рамки окна
    /// </summary>
    private void MainBorder_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }

    /// <summary>
    /// Метод для обработки зажатия TitleBar окна
    /// </summary>
    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
    
    /// <summary>
    /// Метод для обработки нажатие на кнопку сворачивания окна
    /// </summary>
    private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
    {
        if (VisualRoot is Window window)
        {
            window.WindowState = WindowState.Minimized;
        }
    }

    /// <summary>
    /// Метод для обработки нажатие на кнопку закрытия окна
    /// </summary>
    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        // todo: отображать уведомление один раз до закрытия приложения
        _systemNotificationService.SendNotification("Приложение было свернуто", "Вернуться можно при помощи панели значков в левом нижнем углу");
        Hide();
    }

    /// <summary>
    /// Метод для открытия меню управления звуками
    /// </summary>
    private void ManageSoundSetsButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Метод для добавления новой клавиши
    /// </summary>
    private void AddKeyButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Метод для прослушивания пака звуков
    /// </summary>
    private void ListenButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}