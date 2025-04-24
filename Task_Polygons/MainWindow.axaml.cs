using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using System;
using System.IO;

namespace Task_Polygons
{
    public partial class MainWindow : Window
    {
        private bool _pointerPressedInMenu;
        private CustomControl _cc;
        private GraphWindow _graphWindow; 
        private RadiusWindow _radiusWindow;
        private ColorWindow _colorWindow;
        private string _currentFilePath;
        private bool _saved;

        public MainWindow()
        {
            InitializeComponent();

            _cc = this.Find<CustomControl>("myCC");

            ShapeTypes.ItemsSource = new string[] {"Circle", "Square", "Triangle"};
            ShapeTypes.SelectedIndex = 0;

            DrawShellAlgs.ItemsSource = new string[] {"Defenition", "Jarvis"};
            DrawShellAlgs.SelectedIndex = 1;

            _saved = true;
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

                _saved = false;
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
            else if (_graphWindow.WindowState == WindowState.Minimized)
            {
                _graphWindow.WindowState = WindowState.Normal;
            }
            else if (!_graphWindow.IsActive)
            {
                _graphWindow.Activate();
            }
        }

        private void Menu_RadiusSettingSelected(object sender, PointerPressedEventArgs e)
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
            else if (_radiusWindow.WindowState == WindowState.Minimized)
            {
                _radiusWindow.WindowState = WindowState.Normal;
            }
            else if (!_radiusWindow.IsActive)
            {
                _radiusWindow.Activate();
            }
        }

        private void Menu_ColorSettingSelected(object sender, PointerPressedEventArgs e)
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

        private void Window_UpdateRadius(object sender, EventArgs e)
        {
            _cc.UpdateRadius(((RadiusEventArgs)e).Radius);
            _saved = false;
        }

        private void Window_UpdateColor(object sender, EventArgs e)
        {
            _cc.UpdateColor(((ColorEventArgs)e).Color);
            _saved = false;
        }

        private async void Menu_New(object sender, PointerPressedEventArgs e)
        {
            if (!_saved)
            {
                AskToSave();
            }

            _currentFilePath = null;
            _saved = true;
        }

        private async void Menu_Open(object sender, PointerPressedEventArgs e)
        {
            if (!_saved)
            {
                AskToSave();
            }

            var topLevel = GetTopLevel(this);

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open File",
                AllowMultiple = false
            });

            if (files.Count >= 1)
            {
                _cc.LoadState(files[0].Path.AbsolutePath.ToString());
                _currentFilePath = files[0].Path.AbsolutePath.ToString();
            }
        }

        private async void Menu_Save(object sender, PointerPressedEventArgs e)
        {
            if (_currentFilePath == null)
            {
                Menu_SaveAs(sender, e);
            }
            else
            {
                _cc.SaveState(_currentFilePath);
                _saved = true;
            }
        }

        private async void Menu_SaveAs(object sender, PointerPressedEventArgs e)
        {
            var topLevel = GetTopLevel(this);

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save As"
            });

            if (file is not null)
            {
                _cc.SaveState(file.Path.AbsolutePath.ToString());
                _currentFilePath = file.Path.AbsolutePath.ToString();
                _saved = true;
            }
        }

        private void Menu_Exit(object sender, PointerPressedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, WindowClosingEventArgs e)
        {
            if (!_saved)
            {
                AskToSave();
            }

            if (_graphWindow != null)
            {
                _graphWindow.MainWindowClosing();
            }
            if (_radiusWindow != null)
            {
                _radiusWindow.MainWindowClosing();
            }
        }

        private void AskToSave()
        {
            UnsavedChangesWindow unsavedChangesWindow = new UnsavedChangesWindow();
            unsavedChangesWindow.ShowDialog(this);
        }
    }
}