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
        private Pen _pen = new Pen(Brushes.Green);
        private List<Shape> _shapes = [];
        private bool _drawGraph = false;
        Point[] _defenitionPoints, _jarvisPoints;

        public override void Render(DrawingContext drawingContext)
        {
            if (_shapes.Count >= 3)
            {
                DrawShellJarvis(drawingContext);
            }

            foreach (var shape in _shapes)
            {
                shape.Draw(drawingContext);
            }

            if (_drawGraph)
            {
                DrawGraph(drawingContext);
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

        public void SwitchGraph()
        {
            _drawGraph = !_drawGraph;

            if (_defenitionPoints == null)
            {
                MakeGraph();
            }
        }

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

            Shape curShape = firstShape, prevShape = new Circle(firstShape.X + 100, firstShape.Y), tempShape;
            double minCos, cos;

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
                prevShape.IsShell = true;
                curShape = tempShape;
                
                if (drawingContext != null)
                {
                    drawingContext.DrawLine(_pen, new Point(prevShape.X, prevShape.Y), new Point(curShape.X, curShape.Y));
                }

                if (curShape == firstShape)
                {
                    break;
                }
            }
        }

        private void RemoveNonShell()
        {
            _shapes = _shapes.FindAll(x => x.IsShell);
        }

        private void MakeGraph()
        {
            Point[] defenitionPoints = new Point[12];
            Point[] jarvisPoints = new Point[12];

            int defenitionX, defenitionY, jarvisX, jarvisY;

            for (int shapeCount = 3; shapeCount < 300; shapeCount += 25)
            {
                jarvisX = 60 + shapeCount;
                jarvisY = 600 - AlgTime.JarvisTime(shapeCount);

                defenitionX = 60 + shapeCount;
                defenitionY = 600 - AlgTime.DefenitionTime(shapeCount);

                defenitionPoints[(shapeCount - 3) / 25] = new Point(defenitionX, defenitionY);
                jarvisPoints[(shapeCount - 3) / 25] = new Point(jarvisX, jarvisY);
            }

            _defenitionPoints = defenitionPoints;
            _jarvisPoints = jarvisPoints;
        }

        private void DrawGraph(DrawingContext drawingContext)
        {
            Pen defenitionPen = new Pen(new SolidColorBrush(Colors.Red));
            Pen jarvisPen = new Pen(new SolidColorBrush(Colors.Yellow));

            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White)), new Point(50, 610), new Point(50, 160));
            drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.White)), new Point(50, 610), new Point(500, 610));

            Geometry defenitionGeometry = new PolylineGeometry(_defenitionPoints, false);
            Geometry jarvisGeometry = new PolylineGeometry(_jarvisPoints, false);

            drawingContext.DrawGeometry(null, defenitionPen, defenitionGeometry);
            drawingContext.DrawGeometry(null, jarvisPen, jarvisGeometry);
        }
    }
}
