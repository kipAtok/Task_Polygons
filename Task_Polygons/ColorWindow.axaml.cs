using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Task_Polygons;

public partial class ColorWindow : Window
{
    private Color _initialColor;

    public event ColorChangedHandler ColC;

    public ColorWindow(Color color)
    {
        InitializeComponent();

        _initialColor = color;
        ColorWindow_ColorPicker.Color = color;
    }

    private void OkButton_Pressed(object sender, RoutedEventArgs e)
    {
        if (ColC != null)
        {
            ColC(this, new ColorEventArgs(ColorWindow_ColorPicker.Color));
        }

        Close();
    }

    private void CancelButton_Pressed(object sender, RoutedEventArgs e)
    {
        ColorWindow_ColorPicker.Color = _initialColor;
    }
}