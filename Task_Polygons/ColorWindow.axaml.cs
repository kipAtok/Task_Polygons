using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

namespace Task_Polygons;

public partial class ColorWindow : Window
{
    private bool _mainWindowClosing;

    public event ColorChangedHandler ColC;

    public ColorWindow()
    {
        InitializeComponent();
    }

    private void Window_Closing(object sender, WindowClosingEventArgs e)
    {
        if (!_mainWindowClosing)
        {
            ((Window)sender).Hide();
            e.Cancel = true;
        }
    }

    private void ColorPicker_ColorChanged(object sender, ColorChangedEventArgs e)
    {
        if (ColC != null)
        {
            ColC(this, new ColorEventArgs(e.NewColor));
        }
    }
}