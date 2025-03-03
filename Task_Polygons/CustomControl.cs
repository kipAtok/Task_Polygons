using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Task_Polygons
{
    public class CustomControl : UserControl
    {
        private int _x, _y;
        private string _shapeType;
        private List<Shape> _shapes = [];

        public override void Render(DrawingContext drawingContext)
        {
            if (_shapes.Count >= 3)
            {
                //DrawShell(drawingContext);
                DrawShellJarvis(drawingContext);
            }
            foreach (var shape in _shapes)
            {
                shape.Draw(drawingContext);
            }
        }

        public void LeftClick(int x, int y)
        {
            bool isInside = false;

            foreach (var shape in _shapes)
            {
                if (shape.IsInside(x, y))
                {
                    isInside = true;
                    shape.IsMoving = true;
                }
            }
            if (!isInside) 
            {
                if (_shapeType == "Circle")
                {
                    _shapes.Add(new Circle(x, y));
                }
                else if (_shapeType == "Square")
                {
                    _shapes.Add(new Square(x, y));
                }
                else if (_shapeType == "Triangle")
                {
                    _shapes.Add(new Triangle(x, y));
                }
            }
            if (_shapes.Count >= 3)
            {
                //DrawShell(null);
                DrawShellJarvis(null);
                if (!_shapes.Last().IsShell)
                {
                    foreach (var shape in _shapes)
                    {
                        shape.IsMoving = true;
                    }
                }
                RemoveNonShell();
            }

            _x = x;
            _y = y;

            InvalidateVisual();
        }

        public void RightClick(int x, int y)
        {
            Shape shapeToRemove = null;

            foreach (var shape in _shapes)
            {
                if (shape.IsInside(x, y))
                {
                    shapeToRemove = shape;
                }
            }
            if (shapeToRemove != null)
            {
                _shapes.Remove(shapeToRemove);
            }

            InvalidateVisual();
        }

        public void Move(int x, int y)
        {
            foreach (var shape in _shapes)
            {
                if (shape.IsMoving)
                {
                    shape.Move(x - _x, y - _y);
                }
            }

            _x = x;
            _y = y;

            InvalidateVisual();
        }

        public void Release(int x, int y)
        {
            foreach (var shape in _shapes)
            {
                if (shape.IsMoving)
                {
                    shape.IsMoving = false;
                }
            }

            _x = x;
            _y = y;
            
            RemoveNonShell();

            InvalidateVisual();
        }

        public void ChangeShapeType(string shapeType)
        {
            _shapeType = shapeType;
        }

        //private void DrawShell(DrawingContext drawingContext)
        //{
        //    int i1 = 0, i2, i3;
        //    double k, b;
        //    bool above, below;
        //    Pen pen = new Pen(Brushes.Green, 1);

        //    foreach (var shape in _shapes)
        //    {
        //        shape.IsShell = false;
        //    }
        //    foreach (var shape1 in _shapes)
        //    {
        //        i2 = 0;

        //        foreach(var shape2 in _shapes)
        //        {
        //            if (i2 <= i1)
        //            {
        //                i2++;
        //                continue;
        //            }

        //            i3 = 0;
        //            above = false;
        //            below = false;

        //            foreach (var shape3 in _shapes)
        //            {
        //                if (i3 != i1 && i3 != i2)
        //                {
        //                    if (shape1.X != shape2.X)
        //                    {
        //                        k = (double)(shape2.Y - shape1.Y) / (shape2.X - shape1.X);
        //                        b = shape1.Y - shape1.X * k;

        //                        if (shape3.Y < k * shape3.X + b)
        //                        {
        //                            above = true;
        //                        }
        //                        else if (shape3.Y > k * shape3.X + b)
        //                        {
        //                            below = true;
        //                        }
        //                    } 
        //                    else
        //                    {
        //                        if (shape3.X < shape1.X)
        //                        {
        //                            above = true;
        //                        }
        //                        else if (shape3.X > shape1.X)
        //                        {
        //                            below = true;
        //                        }
        //                    }
        //                }

        //                i3++;
        //            }
        //            if (above != below)
        //            {
        //                shape1.IsShell = true;
        //                shape2.IsShell = true;

        //                if (drawingContext != null)
        //                {
        //                    drawingContext.DrawLine(pen, new Point(shape1.X, shape1.Y), new Point(shape2.X, shape2.Y));
        //                }
        //            }

        //            i2++;
        //        }

        //        i1++;
        //    }

        //}

        private double Cos(Shape shape1, Shape shape2, Shape shape3)
        {
            int x1 = shape1.X, y1 = shape1.Y,
                x2 = shape2.X, y2 = shape2.Y,
                x3 = shape3.X, y3 = shape3.Y;

            double length12 = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)),
                   length23 = Math.Sqrt(Math.Pow(x2 - x3, 2) + Math.Pow(y2 - y3, 2)),
                   length31 = Math.Sqrt(Math.Pow(x3 - x1, 2) + Math.Pow(y3 - y1, 2));

            return (Math.Pow(length31, 2) - Math.Pow(length12, 2) - Math.Pow(length23, 2)) / (-2 * length12 * length23);
        }

        private void DrawShellJarvis(DrawingContext drawingContext)
        {
            Shape firstShape = _shapes.OrderBy(x => x.Y).ThenBy(x => x.X).Last();
            List<Shape> shapes = [firstShape];

            Shape curShape = firstShape, prevShape = new Circle(firstShape.X + 100, firstShape.Y), tempShape;
            double minCos = 1, cos;

            foreach (var shape in _shapes)
            {
                shape.IsShell = false;
            }

            while (true)
            {
                minCos = 1;
                tempShape = null;

                foreach (var shape in _shapes)
                {
                    if (shape != prevShape)
                    {
                        cos = Cos(shape, curShape, prevShape);

                        if (cos < minCos)
                        {
                            minCos = cos;
                            tempShape = shape;
                        }
                    }
                }

                prevShape = curShape;
                curShape = tempShape;
                shapes.Add(curShape);

                if(curShape == firstShape)
                {
                    break;
                }
            }

            foreach (var shape in shapes)
            {
                shape.IsShell = true;
            }

            if (drawingContext != null)
            {
                List<Point> points = [];

                foreach (var shape in shapes)
                {
                    points.Add(new Point(shape.X, shape.Y));
                }

                Brush brush = new SolidColorBrush(Colors.Green);
                Pen pen = new Pen(brush);
                PolylineGeometry geometry = new PolylineGeometry(points, false);

                drawingContext.DrawGeometry(brush, pen, geometry);
            }
        }

        private void RemoveNonShell()
        {
            List<Shape> shapes = [];

            foreach (var shape in _shapes)
            {
                if (shape.IsShell)
                {
                    shapes.Add(shape);
                }
            }

            _shapes = shapes;
        }
    }
}
