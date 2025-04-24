
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Task_Polygons;

public partial class UnsavedChangesWindow : Window
{
    public UnsavedChangesWindow()
    {
        InitializeComponent();
    }

    private void Window_Closing(object sender, WindowClosingEventArgs e)
    {
        ((Window)sender).Hide();
        e.Cancel = true;
    }

    private void SaveButton_Pressed(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void DontSaveButton_Pressed(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CancelButton_Pressed(object sender, RoutedEventArgs e)
    {
        Close();
    }
}