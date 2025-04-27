
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Task_Polygons;

public partial class UnsavedChangesWindow : Window
{ 
    public UnsavedChangesWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_Pressed(object sender, RoutedEventArgs e)
    {
        Close("Save");
    }

    private void DontSaveButton_Pressed(object sender, RoutedEventArgs e)
    {
        Close("DontSave");
    }

    private void CancelButton_Pressed(object sender, RoutedEventArgs e)
    {
        Close(null);
    }
}