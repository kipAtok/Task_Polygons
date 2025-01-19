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
            CC.Click((int)e.GetPosition(CC).X, (int)e.GetPosition(CC).Y);
        }

        private void Window_PointerMoved(object? sender, PointerEventArgs e)
        {
            CustomControl CC = this.Find<CustomControl>("myCC");
        }

        private void Window_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            CustomControl CC = this.Find<CustomControl>("myCC");
        }
    }
}