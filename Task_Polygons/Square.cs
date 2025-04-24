using Avalonia;
using Avalonia.Media;
using ProtoBuf;
using System;

namespace Task_Polygons
{
    [ProtoContract(SkipConstructor = true)]
    sealed class Square : Shape
    {
        public Square(int x, int y) : base(x, y) { }

        public override void Draw(DrawingContext drawingContext)
        {
            Brush brush = new SolidColorBrush(_color);
            double side = _r * Math.Sqrt(2);

            drawingContext.DrawRectangle(brush, null, new Rect(_x - side / 2, _y - side / 2, side, side));
        }

        public override bool IsInside(int x, int y)
        {
            double side = _r * Math.Sqrt(2);

            return (Math.Abs(_x - x) <= side / 2) & (Math.Abs(_y - y) <= side / 2);
        }
    }
}

