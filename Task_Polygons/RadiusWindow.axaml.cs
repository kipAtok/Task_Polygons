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

    public event RadiusChangedHandler RC;

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
        if (RC != null)
        {
            RC(this, new RadiusEventArgs((int)e.NewValue));
        }
    }
}