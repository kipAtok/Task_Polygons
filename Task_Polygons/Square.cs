using Avalonia;
using Avalonia.Media;
using System;

namespace Task_Polygons
{
    sealed class Square : Shape
    {
        public Square(int x, int y) : base(x, y) { }

        public override void Draw(DrawingContext drawingContext)
        {
            Pen pen = new Pen(Brushes.Green, 1);
            Brush brush = new SolidColorBrush(Colors.Black);
            double side = _r * Math.Sqrt(2);

            drawingContext.DrawRectangle(brush, pen, new Rect(_x - side / 2, _y - side / 2, side, side));
        }
    }
}

