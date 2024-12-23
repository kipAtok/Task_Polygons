using Avalonia;
using Avalonia.Media;
using System;

namespace Task_2
{
    sealed class Triangle : Shape
    {
        public Triangle(int x, int y) : base(x, y) { }

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.Green, 1);
            Brush brush = new SolidColorBrush(Colors.Black);
            double side = _r * Math.Sqrt(3);
            Point[] points = 
            [
                new Point(_x - side / 2, _y + _r / 2),
                new Point(_x, _y - _r),
                new Point(_x + side / 2, _y + _r / 2),
                new Point(_x - side / 2, _y + _r / 2)
            ];
            PolylineGeometry geometry = new PolylineGeometry(points, true);

            drawingContext.DrawGeometry(brush, pen, geometry);
        }
    }
}