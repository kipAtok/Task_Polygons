using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Task_Polygons
{
    public partial class GraphWindow : Window
    {
        private bool _mainWindowClosing;

        public GraphWindow()
        {
            InitializeComponent();
        }

        public void MainWindowClosing()
        {
            _mainWindowClosing = true;
        }

        private void Menu_RefreshGraph(object sender, PointerPressedEventArgs e)
        {
            GraphControl gc = this.Find<GraphControl>("myGC");
            gc.RefreshGraph();
        }

        private void Menu_SwitchMarking(object sender, PointerPressedEventArgs e)
        {
            GraphControl gc = this.Find<GraphControl>("myGC");
            gc.SwitchMarking();
        }

        private void Window_Closing(object sender, WindowClosingEventArgs e)
        {
            if (!_mainWindowClosing)
            {
                ((Window)sender).Hide();
                e.Cancel = true;
            }
        }
    }
}