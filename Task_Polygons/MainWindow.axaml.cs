using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Diagnostics;

namespace Task_Polygons
{
    public partial class MainWindow : Window
    {
        private bool _pointerPressedInMenu;
        CustomControl _cc;
        private GraphWindow _graphWindow; 
        private RadiusWindow _radiusWindow;
        private ColorWindow _colorWindow;

        public MainWindow()
        {
            InitializeComponent();

            _cc = this.Find<CustomControl>("myCC");

            ShapeTypes.ItemsSource = new string[] {"Circle", "Square", "Triangle"};
            ShapeTypes.SelectedIndex = 0;

            DrawShellAlgs.ItemsSource = new string[] {"Defenition", "Jarvis"};
            DrawShellAlgs.SelectedIndex = 1;

            Settings.ItemsSource = new string[] {"Radius", "Color"};
        }

        private void Window_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (!_pointerPressedInMenu)
            {
                int x = (int)e.GetPosition(_cc).X, y = (int)e.GetPosition(_cc).Y;
                if (e.GetCurrentPoint(_cc).Properties.IsRightButtonPressed)
                {
                    _cc.RightClick(x, y);
                }
                else if (e.GetCurrentPoint(_cc).Properties.IsLeftButtonPressed)
                {
                    _cc.LeftClick(x, y);
                }
            }
        }

        private void Window_PointerMoved(object sender, PointerEventArgs e)
        {
            int x = (int)e.GetPosition(_cc).X, y = (int)e.GetPosition(_cc).Y;
            _cc.Move(x, y);
        }

        private void Window_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _pointerPressedInMenu = false;
            int x = (int)e.GetPosition(_cc).X, y = (int)e.GetPosition(_cc).Y;
            _cc.Release(x, y);
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
            _cc.ChangeShapeType(e.AddedItems[0].ToString());
        }

        private void Menu_DrawShellAlgChanged(object sender, SelectionChangedEventArgs e)
        {
            _cc.ChangeDrawShellAlg(e.AddedItems[0].ToString());
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

        private void Menu_SettingSelected(object sender, SelectionChangedEventArgs e)
        {
            if (Settings.SelectedItem == "Radius")
            {
                if (_radiusWindow == null)
                {
                    _radiusWindow = new RadiusWindow();
                    _radiusWindow.RC += Window_UpdateRadius;
                    _radiusWindow.Show();
                }
                else if (!_radiusWindow.IsVisible)
                {
                    _radiusWindow.Show();
                }
                else if (!_radiusWindow.IsActive)
                {
                    _radiusWindow.Activate();
                } 
            }
            else if (Settings.SelectedItem == "Color")
            {
                if (_colorWindow == null)
                {
                    _colorWindow = new ColorWindow();
                    _colorWindow.ColC += Window_UpdateColor;
                    _colorWindow.ShowDialog(this);
                }
                else if (!_colorWindow.IsVisible)
                {
                    _colorWindow.ShowDialog(this);
                }
            }
            Settings.SelectedIndex = -1;
        }

        private void Window_UpdateRadius(object sender, EventArgs e)
        {
            _cc.UpdateRadius(((RadiusEventArgs)e).Radius);
        }

        private void Window_UpdateColor(object sender, EventArgs e)
        {
            _cc.UpdateColor(((ColorEventArgs)e).Color);
        }

        private void Window_Closing(object sender, WindowClosingEventArgs e)
        {
            if (_graphWindow != null)
            {
                _graphWindow.MainWindowClosing();
            }
            if (_radiusWindow != null)
            {
                _radiusWindow.MainWindowClosing();
            }
        }
    }
}