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
            CustomControl CC = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(CC).X, y = (int)e.GetPosition(CC).Y;
            if (e.GetCurrentPoint(CC).Properties.IsRightButtonPressed)
            {
                CC.RightClick(x, y);
            }
            else
            {
                CC.LeftClick(x, y);
            }
        }

        private void Window_PointerMoved(object? sender, PointerEventArgs e)
        {
            CustomControl CC = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(CC).X, y = (int)e.GetPosition(CC).Y;
            CC.Move(x, y);
        }

        private void Window_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            CustomControl CC = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(CC).X, y = (int)e.GetPosition(CC).Y;
            CC.Release(x, y);
        }
    }
}