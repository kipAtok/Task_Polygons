using Avalonia.Controls;
using Avalonia.Media;

namespace Task_2
{
    public class CustomControl : UserControl
    {
        public override void Render(DrawingContext drawingContext)
        {
            Shape[] shapes = [new Circle(100, 100), new Square(200, 100), new Triangle(300, 100),
                                new Circle(100, 200), new Circle(200, 200), new Square(200, 200), new Circle(300, 200), new Triangle(300, 200),
                                new Circle(200, 300), new Square(200, 300), new Triangle(200, 300)];

            foreach (var shape in shapes)
            {
                shape.Draw(drawingContext);
            }
        }
    }
}
