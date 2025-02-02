using Avalonia.Controls;
using Avalonia.Input;
using System.Diagnostics;

namespace Task_Polygons
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            CustomControl cc = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(cc).X, y = (int)e.GetPosition(cc).Y;
            if (e.GetCurrentPoint(cc).Properties.IsRightButtonPressed)
            {
                cc.RightClick(x, y);
            }
            else
            {
                cc.LeftClick(x, y);
            }
        }

        private void Window_PointerMoved(object? sender, PointerEventArgs e)
        {
            CustomControl cc = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(cc).X, y = (int)e.GetPosition(cc).Y;
            cc.Move(x, y);
        }

        private void Window_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            CustomControl cc = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(cc).X, y = (int)e.GetPosition(cc).Y;
            cc.Release(x, y);
        }
    }
}