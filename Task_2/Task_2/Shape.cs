using Avalonia.Media;

namespace Task_2
{
    abstract class Shape
    {
        protected int _x, _y;
        protected static int _r;

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
    }
}

