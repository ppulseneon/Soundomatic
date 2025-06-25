using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Soundomatic.Views;

/// <summary>
/// Основное окно приложения
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += MainWindowClosing;
    }

    /// <summary>
    /// Метод для обработки события закрытия окна
    /// </summary>
    private void MainWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    private void Content_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Помечаем событие как обработанное.
        // Это предотвратит его "всплытие" к родительскому Border.
        // Таким образом, DraggableBorder_PointerPressed не будет вызван,
        // если клик произошел на Grid или его содержимом.
        e.Handled = true;

        // Если вам нужно, чтобы какие-то элементы внутри Grid все же инициировали перетаскивание,
        // вам пришлось бы не ставить e.Handled = true для них, либо иметь более сложную логику.
        // Но для задачи "тащить только за Border" этого достаточно.
    }

    
    private void MainBorder_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    private void MinimizeButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void ManageSoundSetsButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void AddKeyButton_Click(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}