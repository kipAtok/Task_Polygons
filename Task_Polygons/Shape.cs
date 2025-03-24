using Avalonia.Media;
using System.Diagnostics;

namespace Task_Polygons
{
    abstract class Shape
    {
        protected int _x, _y;
        protected static int _r;
        public bool IsMoving { get; set; }
        public bool IsShell { get; set; } = true;

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        protected Shape(int x, int y)
        {
            _x = x;
            _y = y;
        }

        static Shape()
        {
            _r = 25;
        }

        public abstract void Draw(DrawingContext drawingContext);

        public abstract bool IsInside(int x, int y);

        public virtual void Move(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public static void Resize(int r)
        {
            _r = r;
        }
    }
}

