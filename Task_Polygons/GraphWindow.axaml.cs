using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace Task_Polygons
{
    public partial class GraphWindow : Window
    {
        public GraphWindow()
        {
            InitializeComponent();
        }

        private void Menu_RefreshGraph(object sender, PointerPressedEventArgs e)
        {
            GraphControl gc = this.Find<GraphControl>("myGC");
            gc.RefreshGraph();
        }
    }
}