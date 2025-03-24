using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using System;
using System.Diagnostics;

namespace Task_Polygons;

public partial class RadiusWindow : Window
{
    private bool _mainWindowClosing;

    public RadiusWindow()
    {
        InitializeComponent();
    }

    public void MainWindowClosing()
    {
        _mainWindowClosing = true;
        Close();
    }

    private void Window_Closing(object sender, WindowClosingEventArgs e)
    {
        if (!_mainWindowClosing)
        {
            ((Window)sender).Hide();
            e.Cancel = true;
        }
    }

    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        (int)((Slider)sender).Value);
    }
}