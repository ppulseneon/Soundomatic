using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Messaging;
using Soundomatic.Messages;
using Soundomatic.ViewModels;

namespace Soundomatic.Views;

/// <summary>
/// Основное окно приложения
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel, IMessenger messenger)
    {
        DataContext = viewModel;
        InitializeComponent();
        
        messenger.Register<HideWindowMessage>(this, (_, _) =>
        {
            HideWindow();
        });
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