using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Soundomatic.ViewModels;
using Soundomatic.Views.Interfaces;

namespace Soundomatic.Views;

/// <summary>
/// Основное окно приложения
/// </summary>
public partial class MainWindow : Window, IHideable
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    /// <summary>
    /// Метод для обработки зажатия содержимого окна
    /// </summary>
    private void Content_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Принимаем событие и ничего не делаем
        e.Handled = true;
    }

    /// <summary>
    /// Метод для обработки зажатия рамки окна
    /// </summary>
    private void MainBorder_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // При зажатии рамки передвигаем окно
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
        // При зажатии TitleBar передвигаем окно
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
    /// Метод для сворачивания окна
    /// </summary>
    public void HideWindow()
    {
        Hide();
    }
}