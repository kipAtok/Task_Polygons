using Avalonia.Media;
using ProtoBuf;

namespace Task_Polygons
{
    [ProtoContract]
    [ProtoInclude(5, typeof(Circle))]
    [ProtoInclude(6, typeof(Square))]
    [ProtoInclude(7, typeof(Triangle))]
    abstract class Shape
    {
        [ProtoMember(1)]
        protected int _x;
        [ProtoMember(2)]
        protected int _y;

        [ProtoMember(3)]
        protected static int _r;
        [ProtoMember(4)]
        protected static Color _color;

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

        public static int R
        {
            get
            {
                return _r;
            }
        }

        public static Color Color
        {
            get
            {
                return _color;
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
            _color = Colors.Green;
        }

        public abstract void Draw(DrawingContext drawingContext);

        public abstract bool IsInside(int x, int y);

        public virtual void Move(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public static void SetRadius(int r)
        {
            _r = r;
        }

        public static void SetColor(Color color)
        {
            _color = color;
        }
    }
}

