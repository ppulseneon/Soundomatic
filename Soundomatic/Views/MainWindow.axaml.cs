using Avalonia.Controls;

namespace Soundomatic.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += MainWindowClosing;
    }

    /// <summary>
    /// ��� �������� ���������� �������� ����, �� �� ������� ������ � ��������
    /// </summary>
    private void MainWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}