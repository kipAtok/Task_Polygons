using Avalonia.Controls;
using Avalonia.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace Task_Polygons
{
    public class CustomControl : UserControl
    {
        private List<Shape> _shapes = 
            [
                new Circle(100, 100), new Square(200, 100), new Triangle(300, 100)
            ];
        public override void Render(DrawingContext drawingContext)
        {
            foreach (var shape in _shapes)
            {
                shape.Draw(drawingContext);
            }
        }

        public void Click(int x, int y)
        {
            foreach (var shape in _shapes)
            {
                Debug.WriteLine(shape.IsInside(x, y));
            }
            Debug.WriteLine("~~~");
        }
    }
}
