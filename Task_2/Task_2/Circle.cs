using Avalonia;
using Avalonia.Media;

namespace Task_2
{
    sealed class Circle : Shape
    {
        public Circle(int x, int y) : base(x, y) { }

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.Green, 1);
            Brush brush = new SolidColorBrush(Colors.Black);

            drawingContext.DrawEllipse(brush, pen , new Point(_x, _y), _r, _r);
        }
    }
}

