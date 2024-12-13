
namespace Task_2
{
    abstract class Shape
    {
        protected int x, y;
        protected static int r;

        protected Shape(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        static Shape()
        {
            r = 25;
        }
    }
}

