using Avalonia;
using Avalonia.Media;

namespace Task_Polygons
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

        public override bool IsInside(int x, int y)
        {
            return (_x - x) * (_x - x) + (_y - y) * (_y - y) <= _r * _r;
        }
    }
}

