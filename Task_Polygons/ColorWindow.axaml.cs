using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.Diagnostics;

namespace Task_Polygons;

public partial class ColorWindow : Window
{
    private bool _mainWindowClosing;
    private Color _lastColor;

    public event ColorChangedHandler ColC;

    public ColorWindow()
    {
        InitializeComponent();
        _lastColor = Colors.Green;
    }

    private void Window_Closing(object sender, WindowClosingEventArgs e)
    {
        if (!_mainWindowClosing)
        {
            ColorPicker.Color = _lastColor;

            ((Window)sender).Hide();
            e.Cancel = true;
        }
    }

    private void OkButton_Pressed(object sender, RoutedEventArgs e)
    {
        _lastColor = ColorPicker.Color;

        if (ColC != null)
        {
            ColC(this, new ColorEventArgs(_lastColor));
        }

        Close();
    }

    private void CancelButton_Pressed(object sender, RoutedEventArgs e)
    {
        ColorPicker.Color = _lastColor;
    }
}