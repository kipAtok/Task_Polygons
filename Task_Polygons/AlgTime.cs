using System;

namespace Task_Polygons
{
    static class AlgTime
    {
        private static Random _random = new Random();

        public static int DefenitionTime(int shapeCount)
        {
            Shape[] shapes = MakeShapes(shapeCount);

            DateTime tic = DateTime.Now;  
            
            int i1 = 0, i2, i3;
            double k, b;
            bool above, below;

            foreach (var shape in shapes)
            {
                shape.IsShell = false;
            }

            foreach (var shape1 in shapes)
            {
                i2 = 0;

                foreach (var shape2 in shapes)
                {
                    if (i2 <= i1)
                    {
                        i2++;
                        continue;
                    }

                    i3 = 0;
                    above = false;
                    below = false;

                    foreach (var shape3 in shapes)
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
                    }

                    i2++;
                }

                i1++;
            }

            return (int)(DateTime.Now - tic).TotalMicroseconds;
        }

        private static double Cos(Shape shape1, Shape shape2, Shape shape3)
        {
            int x1 = shape1.X, y1 = shape1.Y,
                x2 = shape2.X, y2 = shape2.Y,
                x3 = shape3.X, y3 = shape3.Y;

            double length12 = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)),
                   length23 = Math.Sqrt(Math.Pow(x2 - x3, 2) + Math.Pow(y2 - y3, 2)),
                   length31 = Math.Sqrt(Math.Pow(x3 - x1, 2) + Math.Pow(y3 - y1, 2));

            return (Math.Pow(length31, 2) - Math.Pow(length12, 2) - Math.Pow(length23, 2)) / (-2 * length12 * length23);
        }

        public static int JarvisTime(int shapeCount)
        {
            Shape[] shapes = MakeShapes(shapeCount);

            DateTime tic = DateTime.Now;

            Shape firstShape = shapes[0];

            for (int i = 1; i < shapeCount; i++)
            {
                if (shapes[i].Y > firstShape.Y)
                {
                    firstShape = shapes[i];
                }
                else if (shapes[i].Y == firstShape.Y && shapes[i].X > firstShape.X)
                {
                    firstShape = shapes[i];
                }
            }

            Shape curShape = firstShape, prevShape = new Circle(firstShape.X + 100, firstShape.Y), tempShape;
            double minCos, cos;

            foreach (var shape in shapes)
            {
                shape.IsShell = false;
            }

            while (true)
            {
                minCos = 1;
                tempShape = null;

                foreach (var shape in shapes)
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

                if (curShape == firstShape)
                {
                    break;
                }
            }

            return (int)(DateTime.Now - tic).TotalMicroseconds;
        }

        private static Shape[] MakeShapes(int shapeCount)
        {
            Shape[] shapes = new Shape[shapeCount];

            for (int i = 0; i < shapeCount; i++)
            {
                shapes[i] = new Circle(_random.Next(0, 1000), _random.Next(0, 1000));
            }

            return shapes;
        }
    }
}
