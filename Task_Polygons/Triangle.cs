using Avalonia;
using Avalonia.Media;
using System;

namespace Task_Polygons
{
    sealed class Triangle : Shape
    {
        private Point _leftPoint, _topPoint, _rightPoint;

        public Triangle(int x, int y) : base(x, y) 
        {
            double side = _r * Math.Sqrt(3);
            _leftPoint = new Point(_x - side / 2, _y + _r / 2);
            _topPoint = new Point(_x, _y - _r);
            _rightPoint = new Point(_x + side / 2, _y + _r / 2);
        }

        public override void Draw(DrawingContext drawingContext)
        {
            Brush brush = new SolidColorBrush(Colors.Green);
            Point[] points = 
            [
                _leftPoint, _topPoint, _rightPoint, _leftPoint
            ];
            PolylineGeometry geometry = new PolylineGeometry(points, true);

            drawingContext.DrawGeometry(brush, null, geometry);
        }

        public override void Move(int x, int y)
        {
            base.Move(x, y);
            double side = _r * Math.Sqrt(3);
            _leftPoint = new Point(_x - side / 2, _y + _r / 2);
            _topPoint = new Point(_x, _y - _r);
            _rightPoint = new Point(_x + side / 2, _y + _r / 2);
        }

        public override bool IsInside(int x, int y)
        {
            Point point = new Point(x, y);
            double area = Area(_leftPoint, _topPoint, _rightPoint);
            double area1 = Area(point, _topPoint, _rightPoint);
            double area2 = Area(_leftPoint, point, _rightPoint);
            double area3 = Area(_leftPoint, _topPoint, point);

            return Math.Abs(area1 + area2 + area3 - area) < 0.1;
        }

        private double Area(Point point1, Point point2, Point point3)
        {
            return Math.Abs((point1.X * (point2.Y - point3.Y) +
                             point2.X * (point3.Y - point1.Y) +
                             point3.X * (point1.Y - point2.Y)) / 2);
        }
    }
}