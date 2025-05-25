using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Task_Polygons;

public partial class RadiusWindow : Window
{
    private bool _mainWindowClosing;

    public event RadiusChangedHandler RC;

    public RadiusWindow(int r)
    {
        InitializeComponent();

        RadiusWindow_Slider.Value = r;
    }

    public void MainWindowClosing()
    {
        _mainWindowClosing = true;
        Close();
    }

    public void UpdateRadius(int r) 
    { 
        RadiusWindow_Slider.Value = r;
    }

    private void Window_Closing(object sender, WindowClosingEventArgs e)
    {
        if (!_mainWindowClosing)
        {
            ((Window)sender).Hide();
            e.Cancel = true;

            if (RC != null)
            {
                RC(this, new RadiusEventArgs((int)RadiusWindow_Slider.Value, true));
            }
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