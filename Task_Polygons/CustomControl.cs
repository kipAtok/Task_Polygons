using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace Task_Polygons
{
    public class CustomControl : UserControl
    {
        private int _x, _y;
        private List<Shape> _shapes =
        [
            new Circle(100, 100), new Square(200, 100), new Triangle(300, 100), 
            new Circle(100, 200), new Circle(200, 200), new Square(200, 200), new Circle(300, 200), new Triangle(300, 200), 
            new Circle(200, 300), new Square(200, 300), new Triangle(200, 300)
        ];

        public override void Render(DrawingContext drawingContext)
        {
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
                _shapes.Add(new Triangle(x, y));
            }

            _x = x;
            _y = y;

            InvalidateVisual();
        }

        public void RightClick(int x, int y)
        {
            List<Shape> shapes = [];

            foreach (var shape in _shapes)
            {
                if (!shape.IsInside(x, y))
                {
                    shapes.Add(shape);
                }
            }

            _shapes = shapes;

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

            InvalidateVisual();
        }
    }
}
