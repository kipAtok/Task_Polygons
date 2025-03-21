using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Diagnostics;

namespace Task_Polygons
{
    public partial class MainWindow : Window
    {
        private bool _pointerPressedInMenu;
        private GraphWindow _graphWindow; 

        public MainWindow()
        {
            InitializeComponent();

            ShapeTypes.ItemsSource = new string[] {"Circle", "Square", "Triangle"};
            ShapeTypes.SelectedIndex = 0;

            DrawShellAlgs.ItemsSource = new string[] {"Defenition", "Jarvis"};
            DrawShellAlgs.SelectedIndex = 1;
        }

        private void Window_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (!_pointerPressedInMenu)
            {
                CustomControl cc = this.Find<CustomControl>("myCC");
                int x = (int)e.GetPosition(cc).X, y = (int)e.GetPosition(cc).Y;
                if (e.GetCurrentPoint(cc).Properties.IsRightButtonPressed)
                {
                    cc.RightClick(x, y);
                }
                else if (e.GetCurrentPoint(cc).Properties.IsLeftButtonPressed)
                {
                    cc.LeftClick(x, y);
                }
            }
        }

        private void Window_PointerMoved(object sender, PointerEventArgs e)
        {
            CustomControl cc = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(cc).X, y = (int)e.GetPosition(cc).Y;
            cc.Move(x, y);
        }

        private void Window_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _pointerPressedInMenu = false;
            CustomControl cc = this.Find<CustomControl>("myCC");
            int x = (int)e.GetPosition(cc).X, y = (int)e.GetPosition(cc).Y;
            cc.Release(x, y);
        }

        private void Menu_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            _pointerPressedInMenu = true;
        }

        private void Menu_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _pointerPressedInMenu = false;
        }

        private void Menu_ShapeTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomControl cc = this.Find<CustomControl>("myCC");
            cc.ChangeShapeType(e.AddedItems[0].ToString());
        }

        private void Menu_DrawShellAlgChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomControl cc = this.Find<CustomControl>("myCC");
            cc.ChangeDrawShellAlg(e.AddedItems[0].ToString());
        }

        private void Menu_DrawGraph(object sender, PointerPressedEventArgs e)
        {
            if (_graphWindow == null)
            {
                _graphWindow = new GraphWindow();
                _graphWindow.Show();
            }
            else if (!_graphWindow.IsVisible)
            {
                _graphWindow.Show();
            }
        }

        private void Window_Closing(object sender, WindowClosingEventArgs e)
        {
            if (_graphWindow != null)
            {
                _graphWindow.MainWindowClosing();
                _graphWindow.Close();
            }
        }
    }
}