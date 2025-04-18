﻿using Avalonia.Media;
using System.Diagnostics;

namespace Task_Polygons
{
    abstract class Shape
    {
        protected int _x, _y;
        protected static int _r;
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

