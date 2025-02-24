using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;

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
                DrawShell(drawingContext);
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
                DrawShell(null);
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

        private void DrawShell(DrawingContext drawingContext)
        {
            int i1 = 0, i2, i3;
            double k, b;
            bool above, below;
            Pen pen = new Pen(Brushes.Green, 1);

            foreach (var shape in _shapes)
            {
                shape.IsShell = false;
            }
            foreach (var shape1 in _shapes)
            {
                i2 = 0;

                foreach(var shape2 in _shapes)
                {
                    if (i2 <= i1)
                    {
                        i2++;
                        continue;
                    }

                    i3 = 0;
                    above = false;
                    below = false;

                    foreach (var shape3 in _shapes)
                    {
                        if (i3 != i1 && i3 != i2)
                        {
                            if (shape1.X != shape2.X)
                            {
                                k = (double)(shape2.Y - shape1.Y) / (shape2.X - shape1.X);
                                b = shape1.Y - shape1.X * k;

                                if (shape3.Y < k * shape3.X + b)
                                {
                                    above = true;
                                }
                                else if (shape3.Y > k * shape3.X + b)
                                {
                                    below = true;
                                }
                            } 
                            else
                            {
                                if (shape3.X < shape1.X)
                                {
                                    above = true;
                                }
                                else if (shape3.X > shape1.X)
                                {
                                    below = true;
                                }
                            }
                        }

                        i3++;
                    }
                    if (above != below)
                    {
                        shape1.IsShell = true;
                        shape2.IsShell = true;

                        if (drawingContext != null)
                        {
                            drawingContext.DrawLine(pen, new Point(shape1.X, shape1.Y), new Point(shape2.X, shape2.Y));
                        }
                    }

                    i2++;
                }

                i1++;
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
