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

            set
            {
                _r = value;
            }
        }

        public static Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
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

        public virtual Shape Clone()
        {
            Shape shape = (Shape)MemberwiseClone();
            shape.IsMoving = false;
            return shape;
        }
    }
}

